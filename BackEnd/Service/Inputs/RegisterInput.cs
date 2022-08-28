using System;

namespace Service.Inputs
{
    public class RegisterInput
    {
        public Guid? Id { get; set; }
        // [Required]
        // [EmailAddress]
        public string Email { get; set; }
        // [Required]
        public string FullName { get; set; }
        // [Required] 
        // [MinLength(8)]
        public string Password { get; set; }
    }
}