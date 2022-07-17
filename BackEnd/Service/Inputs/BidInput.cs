using System;

namespace Service.Inputs
{
    public class BidInput
    {
        public int AuctionId { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}