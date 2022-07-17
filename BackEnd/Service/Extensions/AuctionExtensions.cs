using System.Linq;
using Domain;
using Service.Outputs;

namespace Service.Extensions
{
    public static class AuctionExtensions
    {
        public static AuctionDetails ToAuctionDetails(this Auction auction)
        {
            return new AuctionDetails
            {
                Id = auction.Id,
                Description = auction.Description,
                StartDate = auction.StartDate,
                EndDate = auction.EndDate,
                // CurrentPrice = auction.CurrentPrice,
                // CurrentBidder = auction.CurrentBidder,
                Images = auction.Images.Select(i => i.DataFiles).ToList(),
                OtherDetails = auction.OtherDetails,
                StartingPrice = auction.StartingPrice,
                Type = auction.Type
            };
        }
    }
}