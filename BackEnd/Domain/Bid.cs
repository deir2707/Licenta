using System;
using Infrastructure.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain
{
    [BsonCollection("bids")]
    public class Bid: IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public Guid AuctionId { get; set; }
        public Guid BidderId { get; set; }
        public int Amount { get; set; }
        
        [BsonIgnore] public Auction Auction { get; set; }
        [BsonIgnore] public User? Bidder { get; set; }
    }
}