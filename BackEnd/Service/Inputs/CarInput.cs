using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Service.Inputs
{
    public class CarInput
    {
        // [Required]
        // [RegularExpression("([0-9]+)")]
        public int StartPrice { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Transmission { get; set; }
        public int EngineCapacity { get; set; }
        public int Mileage { get; set; }
        public string FuelType { get; set; }
        public string Description { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}