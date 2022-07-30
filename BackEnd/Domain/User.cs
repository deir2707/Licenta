using System;
using System.Collections.Generic;
using Infrastructure.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain
{
    [BsonCollection("users")]
    public class User : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    
        public string FullName { get; set; }
    
        public string Email { get; set; }
    
        public string Password { get; set; }
    
        public int Balance { get; set; }
    
        public Guid? AddressId { get; set; }
    
        
        [BsonIgnore] public Address? Address { get; set; }
        [BsonIgnore] public List<Auction> Auctions { get; set; }
        [BsonIgnore] public List<Bid> Bids { get; set; }

    }
}