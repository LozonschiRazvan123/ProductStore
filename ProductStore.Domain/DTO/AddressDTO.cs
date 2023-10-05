using System.ComponentModel.DataAnnotations;

namespace ProductStore.DTO
{
    public class AddressDTO
    {
        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "Street should not exceed 100 characters.")]
        public string Street { get; set; }
        [StringLength(100, ErrorMessage = "City should not exceed 100 characters.")]

        public string City { get; set; }
        [StringLength(100, ErrorMessage = "State should not exceed 100 characters.")]
        public string State { get; set; }
    }
}
