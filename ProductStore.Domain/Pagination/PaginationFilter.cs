using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Framework.Pagination
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string Keyword { get; set; }
        public string SortByField { get; set; }
        public string SortAscending { get; set; }

        /*public PaginationFilter(int pageNumber, int pageSize, string keyword, string sortByField)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 50 ? 50 : pageSize;
            Keyword = keyword;
            SortByField = sortByField;
        }*/
    }
}
