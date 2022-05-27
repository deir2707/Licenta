using System.ComponentModel.DataAnnotations;

namespace Service.Inputs
{
    public class LoginInput
    {
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required] 
        public string Password { get; set; }
    
    }
}