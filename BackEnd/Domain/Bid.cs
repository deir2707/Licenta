using System;

namespace Domain
{
    public class Bid: IEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }
        public int BidderId { get; set; }
        public User Bidder { get; set; }
        public int Amount { get; set; }
    }
}