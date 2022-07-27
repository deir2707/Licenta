using System;
using Infrastructure.Mongo;

namespace Domain
{
    [BsonCollection("bids")]
    public class Bid: IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }
        public Guid BidderId { get; set; }
        public User Bidder { get; set; }
        public int Amount { get; set; }
    }
}