using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Models;
using Infrastructure.Notifications;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Repository;
using Service.Extensions;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public class AuctionService : IAuctionService
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IRepository<Auction> _auctionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Bid> _bidsRepository;

        public AuctionService(INotificationPublisher notificationPublisher,
            ICurrentUserProvider currentUserProvider, IRepository<Auction> auctionRepository,
            IRepository<User> userRepository, IRepository<Bid> bidsRepository)
        {
            _notificationPublisher = notificationPublisher;
            _currentUserProvider = currentUserProvider;
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
            _bidsRepository = bidsRepository;
        }

        public async Task<Guid> CreateAuction(AuctionInput auctionInput)
        {
            var otherDetails = ParseOtherDetails(auctionInput);

            var auction = new Auction
            {
                Title = auctionInput.Title,
                StartingPrice = auctionInput.StartPrice,
                Description = auctionInput.Description,
                StartDate = DateTime.UtcNow,
                EndDate = auctionInput.EndDate ?? DateTime.UtcNow.AddDays(7),
                Type = auctionInput.Type,
                OtherDetails = otherDetails,
                SellerId = _currentUserProvider.UserId,
            };

            auction.Images = ExtractImages(auctionInput.Images, auction.Id);

            await _auctionRepository.InsertOneAsync(auction);

            return auction.Id;
        }

        public async Task<AuctionDetails> GetAuctionDetails(Guid id)
        {
            var auction = await _auctionRepository.FindOneAsync(a => a.Id == id, new Expression<Func<Auction, object>>[]
            {
                a => a.Images,
                a => a.Bids.Select(b => b.Bidder),
            });

            await IncludeBids(auction);

            return auction.ToAuctionDetails();
        }

        public async Task<PaginationOutput<AuctionOutput>> GetAllAuctionDetails(int page, int pageSize)
        {
            var query = _auctionRepository
                .FilterBy(a => a.SellerId != _currentUserProvider.UserId && a.EndDate > DateTime.UtcNow,
                    new Expression<Func<Auction, object>>[]
                    {
                        a => a.Images,
                        a => a.Bids
                    })
                .OrderBy(a => a.EndDate);

            var totalCount = query.Count();

            if (totalCount == 0)
            {
                return new PaginationOutput<AuctionOutput>
                {
                    TotalItems = totalCount,
                };
            }

            var auctions = query.AsQueryable().Page(page, pageSize).ToList();

            foreach (var auction in auctions)
            {
                await IncludeBids(auction);
            }

            var auctionDetails = auctions.Select(a => a.ToAuctionOutput()).ToList();

            return new PaginationOutput<AuctionOutput>
            {
                Items = auctionDetails,
                TotalItems = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<List<AuctionOutput>> GetMyAuctions()
        {
            var auctions = _auctionRepository.FilterBy(a => a.SellerId == _currentUserProvider.UserId,
                    new Expression<Func<Auction, object>>[]
                    {
                        a => a.Images,
                        a => a.Bids
                    }).OrderBy(a => a.EndDate)
                .ToList();

            foreach (var auction in auctions)
            {
                await IncludeBids(auction);
            }

            return auctions.Select(a => a.ToAuctionOutput()).ToList();
        }

        public async Task<List<AuctionOutput>> GetWonAuctions()
        {
            var auctions = _auctionRepository.FilterBy(a => a.BuyerId == _currentUserProvider.UserId,
                    new Expression<Func<Auction, object>>[]
                    {
                        a => a.Images,
                        a => a.Bids
                    }).OrderBy(a => a.EndDate)
                .ToList();

            foreach (var auction in auctions)
            {
                await IncludeBids(auction);
            }

            return auctions.Select(a => a.ToAuctionOutput()).ToList();
        }

        public async Task<Guid> MakeBid(BidInput bidInput)
        {
            var auction = await _auctionRepository.FindByIdAsync(bidInput.AuctionId,
                new Expression<Func<Auction, object>>[]
                {
                    a => a.Bids
                });

            ValidateAuction(auction);

            await IncludeBids(auction);

            var highestBid = auction.Bids?.Where(b => b.AuctionId == bidInput.AuctionId)
                .OrderByDescending(b => b.Amount).FirstOrDefault();

            ValidateBid(bidInput, highestBid, auction);

            var bid = new Bid
            {
                AuctionId = auction.Id,
                Amount = bidInput.BidAmount,
                BidderId = _currentUserProvider.UserId,
            };

            await RefundPreviousHighestBidder(highestBid);

            await _bidsRepository.InsertOneAsync(bid);

            _currentUserProvider.User.Balance -= bidInput.BidAmount;
            await _userRepository.ReplaceOneAsync(_currentUserProvider.User);

            await _notificationPublisher.PublishMessageToUser(new Notification
            {
                Event = NotificationEvents.AuctionBid,
                Data = new BidNotification
                {
                    AuctionId = bidInput.AuctionId,
                    BidAmount = bidInput.BidAmount,
                }
            });

            return bid.Id;
        }

        private async Task IncludeBids(Auction auction)
        {
            if (auction.Bids != null)
                return;

            var bids = _bidsRepository.FilterBy(b => b.AuctionId == auction.Id).ToList();

            foreach (var bid in bids)
            {
                bid.Bidder = await _userRepository.FindOneAsync(b => b.Id == bid.BidderId);
            }

            auction.Bids = bids;
        }

        private Dictionary<string, string> ParseOtherDetails(AuctionInput auctionInput)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(auctionInput.OtherDetails);
        }

        private void ValidateAuction(Auction auction)
        {
            if (auction == null)
            {
                throw new AuctionException(ErrorCode.AuctionNotFound, "Auction not found");
            }

            if (auction.EndDate < DateTime.UtcNow)
            {
                throw new AuctionException(ErrorCode.AuctionEnded, "Auction has ended");
            }

            if (auction.SellerId == _currentUserProvider.UserId)
            {
                throw new AuctionException(ErrorCode.BidOnOwnAuction, "You cannot bid on your own auction");
            }
        }

        private void ValidateBid(BidInput bidInput, Bid? highestBid, Auction auction)
        {
            if (_currentUserProvider.User.Balance < bidInput.BidAmount)
            {
                throw new AuctionException(ErrorCode.InsufficientBalance, "Insufficient balance");
            }

            if (highestBid == null)
            {
                if (bidInput.BidAmount <= auction.StartingPrice)
                {
                    throw new AuctionException(ErrorCode.BidTooSmall,
                        "Bid amount must be greater than the starting bid");
                }
            }
            else
            {
                if (bidInput.BidAmount <= highestBid.Amount)
                {
                    throw new AuctionException(ErrorCode.BidTooSmall,
                        "Bid amount must be greater than the highest bid");
                }

                if (highestBid.BidderId == _currentUserProvider.UserId)
                {
                    throw new AuctionException(ErrorCode.BidOnAlreadyWinningAuction,
                        "You cannot bid on an auction you are already winning");
                }
            }
        }

        private async Task RefundPreviousHighestBidder(Bid? highestBid)
        {
            if (highestBid == null)
            {
                return;
            }

            var previousBidder = await _userRepository.FindByIdAsync(highestBid.BidderId);

            previousBidder.Balance += highestBid.Amount;

            await _userRepository.ReplaceOneAsync(previousBidder);
        }

        private static List<Image> ExtractImages(List<IFormFile> images, Guid auctionId)
        {
            return images.Select(img =>
            {
                var fileName = Path.GetFileName(img.FileName);
                var fileExtension = Path.GetExtension(img.FileName);
                var filePath = string.Concat(fileName, fileExtension);

                var photoEntity = new Image
                {
                    ImageFileName = filePath,
                    FileType = fileExtension,
                    AuctionId = auctionId,
                };

                using var target = new MemoryStream();
                img.CopyTo(target);
                photoEntity.DataFiles = target.ToArray();

                return photoEntity;
            }).ToList();
        }
    }
}