using System.ComponentModel.DataAnnotations;

namespace Service.Inputs
{
    public class AddUserInput
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}