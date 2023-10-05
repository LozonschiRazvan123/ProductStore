using System.ComponentModel.DataAnnotations;

namespace ProductStore.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        [StringLength(20, ErrorMessage = "Name should not exceed 20 characters.")]

        public string Name { get; set; }
        [StringLength(20, ErrorMessage = "State should not exceed 20 characters.")]

        public string Surname { get; set; }
        [EmailAddress(ErrorMessage = "Just introduce your email")]
        public string Email { get; set; }
    }
}
