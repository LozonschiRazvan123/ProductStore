using ProductStore.Models;

namespace ProductStore.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public CustomerDTO Customer { get; set; }
    }
}
