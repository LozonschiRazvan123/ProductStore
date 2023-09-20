using System.ComponentModel.DataAnnotations;

namespace ProductStore.Models
{
    public class CategoryProduct
    {
        [Key]
        public int Id { get; set; }
        public string NameCategory { get; set; }
    }
}
