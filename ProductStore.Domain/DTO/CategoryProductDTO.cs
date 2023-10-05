using System.ComponentModel.DataAnnotations;

namespace ProductStore.DTO
{
    public class CategoryProductDTO
    {
        public int Id { get; set; }
        [Display(Name = "Name category")]
        public string NameCategory { get; set; }
    }
}
