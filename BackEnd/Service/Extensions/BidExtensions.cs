using Domain;
using Service.Outputs;

namespace Service.Extensions
{
    public static class BidExtensions
    {
        public static BidDetails ToBidDetails(this Bid bid)
        {
            return new BidDetails
            {
                Id = bid.Id,
                BidAmount = bid.BidAmount,
                BidDate = bid.BidDate,
                BidderName = bid.Bidder.FullName
            };
        }
    }
}