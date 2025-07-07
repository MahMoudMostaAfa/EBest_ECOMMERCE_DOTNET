using System.ComponentModel.DataAnnotations;

namespace EBest.Models
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; } = "";
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; } = false;
    }
}
