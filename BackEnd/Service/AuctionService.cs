using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repository;
using Service.Extensions;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public class AuctionService : IAuctionService
    {
        private readonly AuctionContext _auctionContext;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly ICurrentUserProvider _currentUserProvider;

        public AuctionService(AuctionContext auctionContext, INotificationPublisher notificationPublisher,
            ICurrentUserProvider currentUserProvider)
        {
            _auctionContext = auctionContext;
            _notificationPublisher = notificationPublisher;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<int> CreateAuction(AuctionInput auctionInput)
        {
            var images = ExtractImages(auctionInput.Images);

            var otherDetails = ParseOtherDetails(auctionInput);

            var auction = new Auction
            {
                Title = auctionInput.Title,
                StartingPrice = auctionInput.StartPrice,
                Description = auctionInput.Description,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                Type = AuctionType.Car,
                OtherDetails = otherDetails,
                SellerId = _currentUserProvider.UserId,
                Images = images
            };

            await _auctionContext.Auctions.AddAsync(auction);
            await _auctionContext.SaveChangesAsync();

            return auction.Id;
        }

        private Dictionary<string,string> ParseOtherDetails(AuctionInput auctionInput)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(auctionInput.OtherDetails);
        }

        public async Task<AuctionDetails> GetAuctionDetails(int id)
        {
            var auction = await _auctionContext.Auctions
                .Include(a => a.Images)
                .Include(a => a.Bids).ThenInclude(b=>b.Bidder)
                .FirstOrDefaultAsync(a => a.Id == id);
            return auction.ToAuctionDetails();
        }

        public async Task<PaginationOutput<AuctionOutput>> GetAllAuctionDetails(int page, int pageSize)
        {
            var query = _auctionContext.Auctions
                .Where(a => a.EndDate > DateTime.UtcNow)
                .OrderBy(a => a.EndDate)
                .Include(a => a.Images)
                .Include(a => a.Bids);

            var totalCount = await query.CountAsync();

            var auctions = await query.Page(page, pageSize).ToListAsync();

            var auctionDetails = auctions.Select(a => a.ToAuctionOutput()).ToList();

            return new PaginationOutput<AuctionOutput>
            {
                Items = auctionDetails,
                TotalItems = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<int> MakeBid(BidInput bidInput)
        {
            var auction = await GetAuction(bidInput.AuctionId);

            ValidateAuction(auction);

            var highestBid = auction.Bids.Where(b => b.AuctionId == bidInput.AuctionId)
                .OrderByDescending(b => b.Amount).FirstOrDefault();

            ValidateBid(bidInput, highestBid, auction);

            var bid = new Bid
            {
                AuctionId = bidInput.AuctionId,
                Amount = bidInput.BidAmount,
                BidderId = _currentUserProvider.UserId,
            };

            _currentUserProvider.User.Balance -= bidInput.BidAmount;

            await _auctionContext.Bids.AddAsync(bid);
            await _auctionContext.SaveChangesAsync();

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

        private void ValidateAuction(Auction auction)
        {
            if (auction.EndDate < DateTime.UtcNow)
            {
                throw new AuctionException(ErrorCode.AuctionEnded, "Auction has ended");
            }

            if (auction.SellerId == _currentUserProvider.UserId)
            {
                throw new AuctionException(ErrorCode.BidOnOwnAuction, "You cannot bid on your own auction");
            }
        }

        private void ValidateBid(BidInput bidInput, Bid highestBid, Auction auction)
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
            }
        }

        private static Dictionary<string, string> ExtractOtherDetails(CarInput carInput)
        {
            return new Dictionary<string, string>
            {
                {"make", carInput.Make},
                {"model", carInput.Model},
                {"transmission", carInput.Transmission},
                {"year", carInput.Year.ToString()},
                {"engineCapacity", carInput.EngineCapacity.ToString()},
                {"fuelType", carInput.FuelType},
                {"mileage", carInput.Mileage.ToString()}
            };
        }

        private static List<Image> ExtractImages(List<IFormFile> images)
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
                };

                using var target = new MemoryStream();
                img.CopyTo(target);
                photoEntity.DataFiles = target.ToArray();

                return photoEntity;
            }).ToList();
        }

        private async Task<Auction> GetAuction(int id)
        {
            var auction = await _auctionContext.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null)
            {
                throw new AuctionException(ErrorCode.AuctionNotFound, "Auction not found");
            }

            return auction;
        }

        private async Task<List<Auction>> GetAllAuctions(int? size = null, int? page = null)
        {
            var query = _auctionContext.Auctions
                .Where(a => a.EndDate > DateTime.UtcNow)
                .OrderBy(a => a.EndDate)
                .Include(a => a.Images)
                .Include(a => a.Bids);

            if (!size.HasValue || !page.HasValue)
            {
                return await query.ToListAsync();
            }

            return await query.Skip((page.Value - 1) * size.Value).Take(size.Value).ToListAsync();
        }
    }
}