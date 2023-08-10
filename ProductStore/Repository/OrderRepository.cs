using ProductStore.Data;
using ProductStore.DTO;
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

        public bool Add(OrderDTO order)
        {
            var orders =  _context.Orders.Select(orderCreate => new Order
            {
                Id = order.Id,
                DateTime = order.DateTime,
                Customer = new Customer
                {
                    Id = order.Customer.Id,
                    Name = order.Customer.Name,
                    Surname = order.Customer.Surname,
                    Email = order.Customer.Email
                }
            }).FirstOrDefault();
            _context.Add(orders);
            return Save();
        }

        public bool Delete(Order order)
        {
            throw new NotImplementedException();
        }

        public bool ExistOrder(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            return _context.Orders.Where(o => o.Id==id).Select(order => new OrderDTO
            {
                Id = order.Id,
                DateTime = order.DateTime,
                Customer = new CustomerDTO
                {
                    Id = order.Customer.Id,
                    Name = order.Customer.Name,
                    Surname = order.Customer.Surname,
                    Email = order.Customer.Email
                }
            }).FirstOrDefault();
        }

        public async Task<IEnumerable<OrderDTO>> GetOrders()
        {
            return _context.Orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                DateTime = order.DateTime,
                Customer = new CustomerDTO
                {
                    Id = order.Customer.Id,
                    Name = order.Customer.Name,
                    Surname = order.Customer.Surname,
                    Email = order.Customer.Email
                }
            }).ToList();
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
