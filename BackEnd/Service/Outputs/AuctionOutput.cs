using System;
using Domain;

namespace Service.Outputs
{
    public class AuctionOutput
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StartingPrice { get; set; }
        public AuctionType Type { get; set; }
        // public IDictionary<string,string> OtherDetails { get; set; }
        public byte[] Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NoOfBids { get; set; }
        public int CurrentPrice { get; set; }
    }
}