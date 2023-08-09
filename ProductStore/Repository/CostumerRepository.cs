using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Interface;
using ProductStore.Models;

namespace ProductStore.Repository
{
    public class CostumerRepository: ICustomerRepository
    {
        private readonly DataContext _context;
        public CostumerRepository(DataContext context)
        {
            _context = context;
        }

        public bool Add(Customer customer)
        {
            _context.Add(customer);
            return Save();
        }

        public bool DeleteCustomer(Customer customer)
        {
            _context.Remove(customer);
            return Save();
        }

        public bool ExistCostumer(int id)
        {
            return _context.Customers.Any(customer => customer.Id == id);
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await _context.Customers.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Customer customer)
        {
            _context.Update(customer);
            return Save();
        }
    }
}
