using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public AuctionService(AuctionContext auctionContext, INotificationPublisher notificationPublisher)
        {
            _auctionContext = auctionContext;
            _notificationPublisher = notificationPublisher;
        }

        public async Task<int> CreateCarAuction(CarInput carInput)
        {
            var otherDetails = ExtractOtherDetails(carInput);

            var images = ExtractImages(carInput.Images);

            var auction = new Auction
            {
                StartingPrice = carInput.StartPrice,
                Description = carInput.Description,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                Type = AuctionType.Car,
                OtherDetails = otherDetails,
                SellerId = carInput.UserId,
                Images = images
            };

            await _auctionContext.Auctions.AddAsync(auction);
            await _auctionContext.SaveChangesAsync();

            return auction.Id;
        }

        public async Task<List<AuctionDetails>> GetAllAuctions()
        {
            var auctions = await _auctionContext.Auctions
                .Include(a => a.Images)
                .ToListAsync();
            return auctions.Select(a => a.ToAuctionDetails()).ToList();
        }

        public async Task<int> MakeBid(BidInput bidInput)
        {
            await _notificationPublisher.PublishMessageToUser(new Notification
            {
                Event = NotificationEvents.AuctionBid,
                Data = new BidNotification
                {
                    AuctionId = bidInput.AuctionId,
                    Amount = bidInput.Amount,
                }
            });

            return 0;
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
    }
}