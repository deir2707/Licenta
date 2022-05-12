using System.Collections.Generic;

namespace Domain
{
    public class User : IEntity
    {
        public int Id { get; set; }
    
        public string FullName { get; set; }
    
        public string Email { get; set; }
    
        public string Password { get; set; }
    
        public int Balance { get; set; }
    
        public int? AddressId { get; set; }
    
        public Address? Address { get; set; }
    
        public ICollection<Auction> Auctions { get; set; }
        public ICollection<Bid> Bids { get; set; }

    }
}