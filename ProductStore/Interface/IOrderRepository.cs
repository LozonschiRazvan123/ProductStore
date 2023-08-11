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
        bool Update(OrderDTO order);
        bool Delete(OrderDTO order);
        bool Save();
    }
}
