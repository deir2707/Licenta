using System.Linq;
using Domain;
using Service.Outputs;

namespace Service.Extensions
{
    public static class AuctionExtensions
    {
        public static AuctionDetails ToAuctionDetails(this Auction auction)
        {
            var highestBid = auction.Bids.OrderByDescending(b => b.BidAmount).FirstOrDefault();
            
            return new AuctionDetails
            {
                Id = auction.Id,
                Description = auction.Description,
                StartDate = auction.StartDate,
                EndDate = auction.EndDate,
                Images = auction.Images.Select(i => i.DataFiles).ToList(),
                OtherDetails = auction.OtherDetails,
                StartingPrice = auction.StartingPrice,
                Type = auction.Type,
                CurrentPrice = highestBid?.BidAmount ?? auction.StartingPrice
            };
        }
    }
}