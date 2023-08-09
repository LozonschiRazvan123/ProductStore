using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomers();
        Task<Customer> GetCustomerById(int id);
        bool ExistCostumer(int id);
        bool Add(Customer customer);
        bool Update(Customer customer);
        bool DeleteCustomer(Customer customer);
        bool Save();
    }
}
