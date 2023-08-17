using ProductStore.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductStore.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public IFormFile? ImageProfile { get; set; }
        public UserRole Role { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
