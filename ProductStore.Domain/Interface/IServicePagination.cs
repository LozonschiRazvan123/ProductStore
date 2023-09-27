using ProductStore.Framework.Page;
using ProductStore.Framework.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Core.Interface
{
    public interface IServicePagination<T>
    {
        Task<PageResult<T>> GetCustomersPagination(IQueryable<T> query, PaginationFilter filter);
    }
}
