using ProductStore.DTO;
using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerDTO>> GetCustomers();
        Task<CustomerDTO> GetCustomerById(int id);
        bool ExistCostumer(int id);
        bool Add(CustomerDTO customer);
        bool Update(CustomerDTO customer);
        bool DeleteCustomer(CustomerDTO customer);
        bool Save();
    }
}
