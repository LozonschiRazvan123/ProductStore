using System.ComponentModel.DataAnnotations;

namespace ProductStore.DTO
{
    public class UserRegisterRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters!")]
        public string Password { get; set; }
        [Required, Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
