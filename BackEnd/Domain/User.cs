using System;
using System.Collections.Generic;
using Infrastructure.Mongo;

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
    
        public Address? Address { get; set; }
    
        public List<Auction> Auctions { get; set; } = new();
        public List<Bid> Bids { get; set; } = new();

    }
}