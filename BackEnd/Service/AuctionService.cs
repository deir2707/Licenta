using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Repository;
using Service.Inputs;

namespace Service
{
    public class AuctionService: IAuctionService
    {
        private AuctionContext _auctionContext;

        public AuctionService(AuctionContext auctionContext)
        {
            _auctionContext = auctionContext;
        }

        public async Task<int> CreateCarAuction(CarInput carInput)
        {
            var otherDetails = new Dictionary<string,string>()
            {
                {"make", carInput.Make},
                {"model", carInput.Model},
                {"transmission", carInput.Transmission},
                {"year", carInput.Year.ToString()},
                {"engineCapacity", carInput.EngineCapacity.ToString()},
                {"fuelType", carInput.FuelType},
                {"mileage", carInput.Mileage.ToString()}
            };
            
            var auction = new Auction
            {
                StartingPrice = carInput.StartPrice,
                Description = carInput.Description,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                Type = AuctionType.Car,
                // Images = new List<ImageFile>
                // {
                //     new(carInput.CarImagesInput.OpenReadStream(), carInput.CarImagesInput)
                // },
                OtherDetails = otherDetails,
                SellerId = carInput.UserId
            };

            var images = new List<IFormFile>
            {
                carInput.CarImagesInput
            };

            var auctionImages = new List<Image>();

            foreach (var image in images)
            {
                var fileName = Path.GetFileName(image.FileName);
                var fileExtension = Path.GetExtension(image.FileName);
                var filePath = String.Concat(fileName, fileExtension);
            
                var photoEntity = new Image
                {
                    AuctionId = auction.Id,
                    ImageFileName = filePath,
                    FileType = fileExtension,
                };

                using (var target = new MemoryStream())
                {
                    image.CopyTo(target);
                    photoEntity.DataFiles = target.ToArray();
                }
                auctionImages.Add(photoEntity);
            }

            auction.Images = auctionImages;

            await _auctionContext.Auctions.AddAsync(auction);
            await _auctionContext.SaveChangesAsync();
            
            return auction.Id;
        }
    }
}