using System;

namespace Service.Outputs
{
    public class BidNotification
    {
        public Guid AuctionId { get; set; }
        public int BidAmount { get; set; }
    }
}