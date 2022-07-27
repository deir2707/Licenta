using System;
using System.Collections.Generic;
using Domain;

namespace Service.Outputs
{
    public class AuctionDetails
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StartingPrice { get; set; }
        public AuctionType Type { get; set; }
        public IDictionary<string,string> OtherDetails { get; set; }
        public List<byte[]> Images { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrentPrice { get; set; }
        public int NoOfBids { get; set; }
        public List<BidDetails> Bids { get; set; }
        public bool IsFinished { get; set; }
        public Guid SellerId { get; set; }
    }
}