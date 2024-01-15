using ProductStore.Core.Validation;
using ProductStore.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductStore.DTO
{
    public class ProductDTO: IValidatableObject
    {
        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "Street should not exceed 100 characters.")]
        public string Street { get; set; }
        [StringLength(100, ErrorMessage = "Name should not exceed 100 characters.")]
        public string Name { get; set; }
        //[CustomProductValidation]
        public int Price { get; set; }

        //public CategoryProductDTO CategoryProduct { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Price <= 0)
            {
                yield return new ValidationResult("Price must be greater than zero.", new[] { nameof(Price) });
            }
        }


    }
}
