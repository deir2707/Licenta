using System;

namespace Service.Outputs
{
    public class BidDetails
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string BidderName { get; set; }
    }
}