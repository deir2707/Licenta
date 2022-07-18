﻿namespace Domain
{
    public class Bid: IEntity
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }
        public int BidderId { get; set; }
        public User Bidder { get; set; }
        public int BidAmount { get; set; }
    }
}