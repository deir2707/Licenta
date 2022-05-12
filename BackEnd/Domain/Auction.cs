using System;

namespace Domain
{

    public class Auction : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StartingPrice { get; set; }
        public int SellerId { get; set; }
        public User Seller { get; set; }
        public int? BuyerId { get; set; }
        public User? Buyer { get; set; }
    }
}