using System.ComponentModel.DataAnnotations;

namespace ProductStore.DTO
{
    public class ResetPassword
    {
        [Required]
        public string Token { get; set; }
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters!")]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
