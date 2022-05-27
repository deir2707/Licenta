using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Service.Inputs
{

    public class IFormFileWrapper
    {
        public IFormFile? File { get; set; }
    }

    public class CarInput
    {
        // [Required]
        // [RegularExpression("([0-9]+)")]
        public string StartPrice { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Transmission { get; set; }
        public string Engine_capacity { get; set; }
        public string Mileage { get; set; }
        public string Fuel_Type { get; set; }
        public IFormFileWrapper FileWrapper { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }

    }
}