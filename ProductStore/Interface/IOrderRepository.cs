using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrderById(int id);
        bool ExistOrder(int id);
        bool Add(Order order);
        bool Update(Order order);
        bool Delete(Order order);
        bool Save();
    }
}
