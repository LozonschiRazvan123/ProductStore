using ProductStore.DTO;
using ProductStore.Framework.Page;
using ProductStore.Framework.Pagination;
using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerDTO>> GetCustomers();
        Task<PageResult<CustomerDTO>> GetCustomersPagination(PaginationFilter filter);
        Task<CustomerDTO> GetCustomerById(int id);
        bool ExistCostumer(int id);
        bool Add(CustomerDTO customer);
        bool Update(CustomerDTO customer);
        bool DeleteCustomer(CustomerDTO customer);
        bool Save();
    }
}
