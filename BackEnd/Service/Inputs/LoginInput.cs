using System.ComponentModel.DataAnnotations;

namespace Service.Inputs
{
    public class LoginInput
    {
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required] 
        [MinLength(8)]
        public string Password { get; set; }
    
    }
}