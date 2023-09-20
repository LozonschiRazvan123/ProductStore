using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;
using System.Net;

namespace ProductStore.Repository
{
    public class CostumerRepository: ICustomerRepository
    {
        private readonly DataContext _context;
        public CostumerRepository(DataContext context)
        {
            _context = context;
        }

        public bool Add(CustomerDTO customerCreateDTO)
        {
            var customer = _context.Customers.Where(a => a.Email == customerCreateDTO.Email).FirstOrDefault();

            if (customer == null)
            {

                var customerCreate = new Customer
                {
                    Id = customerCreateDTO.Id,
                    Name = customerCreateDTO.Name,
                    Surname = customerCreateDTO.Surname,
                    Email = customerCreateDTO.Email
                };
                _context.Add(customerCreate);
                return Save();
            }
            return false;
        }

        public bool DeleteCustomer(CustomerDTO customer)
        {
            var customerDTO = _context.Customers.Find(customer.Id);
            _context.Remove(customerDTO);
            return Save();
        }

        public bool ExistCostumer(int id)
        {
            return _context.Customers.Any(customer => customer.Id == id);
        }

        public async Task<CustomerDTO> GetCustomerById(int id)
        {
            //return await _context.Customers.Where(c => c.Id == id).FirstOrDefaultAsync();

            return _context.Customers.Where(c => c.Id == id).Select(w => new CustomerDTO()
            {
                Id = w.Id,
                Name = w.Name,
                Surname = w.Surname,
                Email = w.Email
            }).FirstOrDefault();
        }

        public async Task<IEnumerable<CustomerDTO>> GetCustomers()
        {
            //return await _context.Customers.ToListAsync();
            return _context.Customers.Select(customer => new CustomerDTO
            {
                Id = customer.Id,
                Name = customer.Name,
                Surname = customer.Surname,
                Email = customer.Email
            }).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(CustomerDTO customer)
        {
            var customerDTO = _context.Customers.Where(c => c.Id == customer.Id).FirstOrDefault();

            if (customerDTO != null)
            {

                customerDTO.Id = customer.Id;
                customerDTO.Name = customer.Name;
                customerDTO.Surname = customer.Surname;
                customerDTO.Email = customer.Email;
                _context.Update(customerDTO);
                return Save();
            }
            return false;
        }
    }
}
