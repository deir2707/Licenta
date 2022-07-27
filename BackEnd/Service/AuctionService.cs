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
                EndDate = DateTime.UtcNow.AddDays(7),
                Type = auctionInput.Type,
                OtherDetails = otherDetails,
                SellerId = _currentUserProvider.UserId,
                Seller = _currentUserProvider.User
            };

            auction.Images = ExtractImages(auctionInput.Images, auction.Id);

            await _auctionRepository.InsertOneAsync(auction);

            return auction.Id;
        }

        private Dictionary<string, string> ParseOtherDetails(AuctionInput auctionInput)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(auctionInput.OtherDetails);
        }

        public async Task<AuctionDetails> GetAuctionDetails(Guid id)
        {
            var auction = await _auctionRepository.FindOneAsync(a => a.Id == id, new Expression<Func<Auction, object>>[]
            {
                a => a.Images,
                a => a.Bids.Select(b => b.Bidder),
            });

            return auction.ToAuctionDetails();
        }

        public Task<PaginationOutput<AuctionOutput>> GetAllAuctionDetails(int page, int pageSize)
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

            var auctions = query.AsQueryable().Page(page, pageSize).ToList();

            var auctionDetails = auctions.Select(a => a.ToAuctionOutput()).ToList();

            return Task.FromResult(new PaginationOutput<AuctionOutput>
            {
                Items = auctionDetails,
                TotalItems = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }

        public async Task<Guid> MakeBid(BidInput bidInput)
        {
            var auction = await _auctionRepository.FindByIdAsync(bidInput.AuctionId, new Expression<Func<Auction, object>>[]
            {
                a => a.Bids
            });

            var user = await _userRepository.FindByIdAsync(_currentUserProvider.UserId);
            
            ValidateAuction(auction, user);

            var highestBid = auction.Bids.Where(b => b.AuctionId == bidInput.AuctionId)
                .OrderByDescending(b => b.Amount).FirstOrDefault();

            ValidateBid(bidInput, highestBid, auction, user);

            var bid = new Bid
            {
                AuctionId = bidInput.AuctionId,
                Amount = bidInput.BidAmount,
                BidderId = user.Id,
                Bidder = user
            };

            auction.Bids.Add(bid);
            await _bidsRepository.InsertOneAsync(bid);
            
            user.Balance -= bidInput.BidAmount;
            await _userRepository.ReplaceOneAsync(user);

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

        public Task<List<AuctionOutput>> GetMyAuctions()
        {
            return Task.FromResult(_auctionRepository
                .FilterBy(a => a.SellerId == _currentUserProvider.UserId,
                    a => a.ToAuctionOutput(),
                    new Expression<Func<Auction, object>>[]
                    {
                        a => a.Images,
                        a => a.Bids
                    })
                .OrderBy(a => a.EndDate)
                .ToList());
        }

        public Task<List<AuctionOutput>> GetWonAuctions()
        {
            return Task.FromResult(_auctionRepository
                .FilterBy(a => a.BuyerId == _currentUserProvider.UserId,
                    a => a.ToAuctionOutput(),
                    new Expression<Func<Auction, object>>[]
                    {
                        a => a.Images,
                        a => a.Bids
                    })
                .OrderBy(a => a.EndDate)
                .ToList());
        }

        private void ValidateAuction(Auction auction, User user)
        {
            if (auction == null)
            {
                throw new AuctionException(ErrorCode.AuctionNotFound, "Auction not found");
            }
            
            if (auction.EndDate < DateTime.UtcNow)
            {
                throw new AuctionException(ErrorCode.AuctionEnded, "Auction has ended");
            }

            if (auction.SellerId == user.Id)
            {
                throw new AuctionException(ErrorCode.BidOnOwnAuction, "You cannot bid on your own auction");
            }
        }

        private void ValidateBid(BidInput bidInput, Bid highestBid, Auction auction, User user)
        {
            if (user.Balance < bidInput.BidAmount)
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
            }
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

        private async Task<Auction> GetAuction(Guid id)
        {
            var auction = await _auctionRepository.FindByIdAsync(id, new Expression<Func<Auction, object>>[]
            {
                a => a.Bids
            });

            if (auction == null)
            {
                throw new AuctionException(ErrorCode.AuctionNotFound, "Auction not found");
            }

            return auction;
        }
    }
}