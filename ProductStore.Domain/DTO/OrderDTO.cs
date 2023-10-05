using ProductStore.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductStore.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
        public CustomerDTO Customer { get; set; }
    }
}
