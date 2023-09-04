using ProductStore.Enum;
using System.ComponentModel.DataAnnotations;

namespace ProductStore.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters!")]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }   
        public UserRole Role { get; set; }
        public byte[]? ImageProfile { get; set; }

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
        /*public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string? VerificationToken { get; set; }*/
    }
}
