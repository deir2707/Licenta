using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Repository;
using Service.Inputs;

namespace Service
{
    public class AuctionService: IAuctionService
    {
        private readonly AuctionContext _auctionContext;

        public AuctionService(AuctionContext auctionContext)
        {
            _auctionContext = auctionContext;
        }

        public async Task<int> CreateCarAuction(CarInput carInput)
        {
            var otherDetails = new Dictionary<string,string>
            {
                {"make", carInput.Make},
                {"model", carInput.Model},
                {"transmission", carInput.Transmission},
                {"year", carInput.Year.ToString()},
                {"engineCapacity", carInput.EngineCapacity.ToString()},
                {"fuelType", carInput.FuelType},
                {"mileage", carInput.Mileage.ToString()}
            };

            var images = carInput.Images.Select(img =>
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
    }
}