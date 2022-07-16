using Microsoft.AspNetCore.Http;

namespace Service.Inputs
{
    public class IFormFileWrapper
    {
        public IFormFile File { get; set; }
    }

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
        // public IFormFileWrapper FileWrapper { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public IFormFile CarImagesInput { get; set; }
    }

    public class CarImagesInput
    {
        public IFormFile File { get; set; }
    }
}