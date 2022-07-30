using System;
using System.Collections.Generic;
using Infrastructure.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain
{
    public enum AuctionType
    {
        Car = 1,
        Painting = 2,
        Antiquity = 3
    }

    [BsonCollection("auctions")]
    public class Auction : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StartingPrice { get; set; }
        public Guid SellerId { get; set; }
        public AuctionType Type { get; set; }
        public Guid? BuyerId { get; set; }
        public IDictionary<string, string> OtherDetails { get; set; }
        
        public List<Image> Images { get; set; }
        [BsonIgnore] public User? Buyer { get; set; }
        [BsonIgnore] public User Seller { get; set; }
        [BsonIgnore] public List<Bid> Bids { get; set; }
    }
}