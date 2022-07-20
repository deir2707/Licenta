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
                Amount = bid.Amount,
                Date = bid.Date,
                BidderName = bid.Bidder.FullName
            };
        }
    }
}