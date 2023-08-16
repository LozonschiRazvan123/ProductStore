using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductStore.Models
{
    public class Product
    {
        [Key] 
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        [ForeignKey("CategoryProduct")]
        public int? CategoryProductId { get; set; }
        public CategoryProduct? CategoryProduct { get; set; }
        public IEnumerable<OrderProduct> OrderProduct { get; set; }
    }
}
