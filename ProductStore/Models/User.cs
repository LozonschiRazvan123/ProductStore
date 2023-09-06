using Microsoft.AspNetCore.Identity;
using ProductStore.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductStore.Models
{
    [Table(name: "Users")]
    public class User: IdentityUser
    {
        public byte[]? ImageProfile { get; set; }
        public UserRole Role { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
