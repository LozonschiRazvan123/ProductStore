using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Framework.Page;
using ProductStore.Framework.Pagination;
using ProductStore.Interface;
using ProductStore.Models;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<PageResult<CustomerDTO>> GetCustomersPagination(PaginationFilter filter)
        {
            var query = _context.Customers
                .Select(customer => new CustomerDTO
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Surname = customer.Surname,
                    Email = customer.Email
                }).AsQueryable();

            if (!string.IsNullOrEmpty(filter.SortByField))
            {
                switch (filter.SortByField.ToLower())
                {
                    case "name":
                        if (filter.SortAscending == "true")
                        {
                            query = query.Where(customer => customer.Name.Contains(filter.Keyword)).OrderBy(customer => customer.Surname);                        
                        }
                        else
                        {
                            query = query.Where(customer => customer.Name.Contains(filter.Keyword)).OrderByDescending(customer => customer.Surname);                        
                        }
                        break;
                    case "surname":
                        if (filter.SortAscending == "true")
                        {
                            query = query.Where(customer => customer.Surname.Contains(filter.Keyword)).OrderBy(customer => customer.Name);
                        }
                        else
                        {
                            query = query.Where(customer => customer.Surname.Contains(filter.Keyword)).OrderByDescending(customer => customer.Name);                        }
                        break;
                    case "email":
                        if (filter.SortAscending == "true")
                        {
                            query = query.Where(customer => customer.Email.Contains(filter.Keyword)).OrderBy(customer => customer.Email);
                        }
                        else
                        {
                            query = query.Where(customer => customer.Email.Contains(filter.Keyword)).OrderByDescending(customer => customer.Email);
                        }
                        break;
                    default:
                        break;
                }
            }

            var totalRecords = await query.CountAsync();

            var customers = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize);

            var result = new PageResult<CustomerDTO>
            {
                Results = customers,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            };

            return result;
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
