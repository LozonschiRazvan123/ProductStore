using ProductStore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Core.Interface
{
    public interface ICustomerRepositoryDapper
    {
        Task<IEnumerable<CustomerDTO>> GetCustomers();
        Task<IEnumerable<CustomerDTO>> GetCustomersId(int customerId);
        Task<bool> UpdateCustomer(int customerId, CustomerDTO customer);
        Task<bool> CreateCustomer(CustomerDTO customer);
        Task DeleteCustomer(int id);
    }
}
