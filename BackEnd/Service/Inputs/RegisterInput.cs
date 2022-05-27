using System.ComponentModel.DataAnnotations;

namespace Service.Inputs
{
    public class RegisterInput
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required] 
        public string Password { get; set; }
    }
}