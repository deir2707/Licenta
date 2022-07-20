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
    }
    public class CarInput2
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Transmission { get; set; }
        public int EngineCapacity { get; set; }
        public int Mileage { get; set; }
        public string FuelType { get; set; }
    }
}