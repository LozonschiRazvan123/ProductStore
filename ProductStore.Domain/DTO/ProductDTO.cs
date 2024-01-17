using ProductStore.Core.Validation;
using ProductStore.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductStore.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "Street should not exceed 100 characters.")]
        public string Street { get; set; }
        [StringLength(100, ErrorMessage = "Name should not exceed 100 characters.")]
        public string Name { get; set; }

        [CustomProductValidation(ErrorMessage = "Price must be greater than zero.")]
        public int Price { get; set; }

        //public CategoryProductDTO CategoryProduct { get; set; }


    }
}
