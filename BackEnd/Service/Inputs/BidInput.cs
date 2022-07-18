using System;

namespace Service.Inputs
{
    public class BidInput
    {
        public int AuctionId { get; set; }
        public int BidAmount { get; set; }
        public DateTime Date { get; set; }
    }
}