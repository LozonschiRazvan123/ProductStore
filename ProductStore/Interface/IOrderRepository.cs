using ProductStore.DTO;
using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDTO>> GetOrders();
        Task<OrderDTO> GetOrderById(int id);
        bool ExistOrder(int id);
        bool Add(OrderDTO order);
        bool Update(Order order);
        bool Delete(Order order);
        bool Save();
    }
}
