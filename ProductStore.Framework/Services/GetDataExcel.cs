using ProductStore.Core.Interface;
using ProductStore.Interface;
using ProductStore.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Framework.Services
{
    public class GetDataExcel : IGetDataExcel
    {
        private IAddressRepository _addressRepository;
        private ICategoryProductRepository _categoryProductRepository;
        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;
        private IProductRepository _productRepository;
        private IUserRepository _userRepository;
        public GetDataExcel(IAddressRepository addressRepository, ICategoryProductRepository categoryProductRepository, IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _addressRepository = addressRepository;
            _categoryProductRepository = categoryProductRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }
        public DataTable GetAddressData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Address";
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Street", typeof(string));
            dt.Columns.Add("City", typeof(string));
            dt.Columns.Add("State", typeof(string));

            var addressData = _addressRepository.GetAddresses();
            foreach (var address in addressData)
            {
                dt.Rows.Add(address.Id, address.Street, address.City, address.State);

            }

            return dt;
        }

        public DataTable GetCategoryProductData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "CategoryProduct";
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("NameCategory", typeof(string));

            var categoryData = _categoryProductRepository.GetCategoryProducts().Result;
            foreach (var category in categoryData)
            {
                dt.Rows.Add(category.Id, category.NameCategory);

            }

            return dt;
        }

        public DataTable GetCustomerData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Customer";
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Surname", typeof(string));
            dt.Columns.Add("Email", typeof(string));

            var categoryData = _customerRepository.GetCustomers().Result;
            foreach (var customer in categoryData)
            {
                dt.Rows.Add(customer.Id, customer.Name, customer.Surname, customer.Email);

            }

            return dt;
        }

        public DataTable GetOrderData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Order";
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("DateTime", typeof(DateTime));

            var categoryData = _orderRepository.GetOrders().Result;
            foreach (var customer in categoryData)
            {
                dt.Rows.Add(customer.Id, customer.DateTime);

            }

            return dt;
        }

        public DataTable GetProductData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Product";
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Price", typeof(int));

            var categoryData = _productRepository.GetProducts().Result;
            foreach (var customer in categoryData)
            {
                dt.Rows.Add(customer.Id, customer.Name, customer.Price);

            }

            return dt;
        }

        public DataTable GetUserData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "User";
            dt.Columns.Add("Id", typeof(Guid));
            dt.Columns.Add("ImageProfil", typeof(byte));
            /*dt.Columns.Add("VericationToken", typeof(string));
            dt.Columns.Add("VerifiedAt", typeof(DateTime));
            dt.Columns.Add("PasswordResetToken", typeof(string));
            dt.Columns.Add("ResetTokenExpires", typeof(DateTime));*/
            dt.Columns.Add("UserName", typeof(string));
            /*            dt.Columns.Add("NormalizedUserName", typeof (string));
                        dt.Columns.Add("Email", typeof(string));*/

            var categoryData = _userRepository.GetUsers().Result;
            foreach (var customer in categoryData)
            {
                dt.Rows.Add(customer.Id, customer.ImageProfile, customer.UserName);

            }

            return dt;
        }
    }
}
