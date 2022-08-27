using System;
using System.Collections.Generic;
using Domain;
using Microsoft.AspNetCore.Http;

namespace Service.Inputs
{
    public class AuctionInput
    {
        public int StartPrice { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<IFormFile> Images { get; set; }
        public AuctionType Type { get; set; }
        public string OtherDetails { get; set; }
        public DateTime? EndDate { get; set; }
    }
}