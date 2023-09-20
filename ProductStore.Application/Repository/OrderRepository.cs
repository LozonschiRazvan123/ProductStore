using Microsoft.EntityFrameworkCore;
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
            var orderToAdd = new Order
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
            };
            _context.Add(orderToAdd);
            return Save();
        }

        public bool Delete(OrderDTO order)
        {
            var orders = _context.Orders.Find(order.Id);
            _context.Remove(orders);
            return Save();
        }

        public bool ExistOrder(int id)
        {
            return _context.Orders.Any(o => o.Id == id);
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
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(OrderDTO order)
        {
            var existingOrder = _context.Orders.Include(o => o.Customer).FirstOrDefault(o => o.Id == order.Id);

            if (existingOrder != null)
            {

                existingOrder.DateTime = order.DateTime;
                existingOrder.Customer.Name = order.Customer.Name;
                existingOrder.Customer.Surname = order.Customer.Surname;
                existingOrder.Customer.Email = order.Customer.Email;

                _context.Update(existingOrder);
                return Save();
            }

            return false;
        }
    }
}
