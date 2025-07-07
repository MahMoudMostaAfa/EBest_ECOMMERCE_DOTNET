using System.ComponentModel.DataAnnotations;

namespace EBest.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "First Name is required"), MaxLength(100)]
        public string FirstName { get; set; } = "";
        [Required(ErrorMessage = "Last Name is required"), MaxLength(100)]
        public string LastName { get; set; } = "";

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = "";

        [Phone(ErrorMessage = "Invalid phone number format"),MaxLength(20)]
        public string? PhoneNumber { get; set; } 

        [Required, MaxLength(200)]
        public string Address { get; set; } = "";

        [Required ,MaxLength(20)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage ="confirm password field is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match"), MaxLength(20)]
        public string ConfirmPassword { get; set; } = "";
    }
}
