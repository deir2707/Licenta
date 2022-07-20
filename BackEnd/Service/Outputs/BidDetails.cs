using System;

namespace Service.Outputs
{
    public class BidDetails
    {
        public int Id { get; set; }
        public int BidAmount { get; set; }
        public DateTime BidDate { get; set; }
        public string BidderName { get; set; }
        
    }
}