using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Core.Interface
{
    public interface IGetDataExcel
    {
        public DataTable GetAddressData();
        public DataTable GetCategoryProductData();
        public DataTable GetCustomerData();
        public DataTable GetOrderData();
        public DataTable GetProductData();
        public DataTable GetUserData();
    }
}
