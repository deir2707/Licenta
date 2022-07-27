using System;

namespace Service.Inputs
{
    public class BidInput
    {
        public Guid AuctionId { get; set; }
        public int BidAmount { get; set; }
        public DateTime Date { get; set; }
    }
}