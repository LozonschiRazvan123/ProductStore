using ProductStore.Data;
using ProductStore.Interface;
using ProductStore.Models;

namespace ProductStore.Repository
{
    public class OrderRepository: IOrderRepository
    {
        private readonly DataContext _context;
        public OrderRepository(DataContext context) 
        {
            _context = context;
        }

        public bool Add(Order order)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Order order)
        {
            throw new NotImplementedException();
        }

        public bool ExistOrder(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetOrders()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
