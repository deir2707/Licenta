using System;
using System.Linq;
using Domain;
using Service.Outputs;

namespace Service.Extensions
{
    public static class AuctionExtensions
    {
        public static AuctionOutput ToAuctionOutput(this Auction auction)
        {
            var highestBid = auction.Bids.OrderByDescending(b => b.Amount).FirstOrDefault();
            
            return new AuctionOutput
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                StartDate = auction.StartDate.ToLocalTime(),
                EndDate = auction.EndDate.ToLocalTime(),
                Image = auction.Images.Select(i => i.DataFiles).FirstOrDefault(),
                StartingPrice = auction.StartingPrice,
                Type = auction.Type,
                NoOfBids = auction.Bids.Count,
                CurrentPrice = highestBid?.Amount ?? auction.StartingPrice,
                IsFinished = auction.EndDate < DateTime.UtcNow
            };
        }
        public static AuctionDetails ToAuctionDetails(this Auction auction)
        {
            var highestBid = auction.Bids.OrderByDescending(b => b.Amount).FirstOrDefault();
            
            return new AuctionDetails
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                StartDate = auction.StartDate.ToLocalTime(),
                EndDate = auction.EndDate.ToLocalTime(),
                Images = auction.Images.Select(i => i.DataFiles).ToList(),
                OtherDetails = auction.OtherDetails,
                StartingPrice = auction.StartingPrice,
                Type = auction.Type,
                NoOfBids = auction.Bids.Count,
                CurrentPrice = highestBid?.Amount ?? auction.StartingPrice,
                Bids = auction.Bids.Select(b=>b.ToBidDetails()).ToList(),
                IsFinished = auction.EndDate < DateTime.UtcNow,
                SellerId = auction.SellerId
            };
        }
    }
}