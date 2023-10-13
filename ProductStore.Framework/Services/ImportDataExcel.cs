using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using ProductStore.Core.Interface;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Framework.Services
{
    public class ImportDataExcel : IImportDataExcel
    {
        private IAddressRepository _addressRepository;
        private DataContext _dataContext;
        private ICategoryProductRepository _categoryProductRepository;
        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;
        private IProductRepository _productRepository;
        private IUserRepository _userRepository;
        public ImportDataExcel(IAddressRepository addressRepository, DataContext dataContext, ICategoryProductRepository categoryProductRepository, ICustomerRepository customerRepository, IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _addressRepository = addressRepository;
            _dataContext = dataContext;
            _categoryProductRepository = categoryProductRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;

        }

        public async Task ImportDataFromExcelCategoryProduct(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        int id = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                        string nameCategory = worksheet.Cells[row, 2].Value.ToString();

                        var categoryProductDTO = new CategoryProductDTO
                        {
                            Id = id,
                            NameCategory = nameCategory
                        };


                        var existingCategoryProduct = await _categoryProductRepository.GetCategoryProductById(categoryProductDTO.Id);

                        if (existingCategoryProduct != null)
                        {
                            existingCategoryProduct.NameCategory = nameCategory;
                        }
                        else
                        {
                            var newCategoryName = new CategoryProduct { NameCategory = nameCategory};
                            _dataContext.CategoryProducts.Add(newCategoryName);
                        }
                    }
                    _dataContext.SaveChanges();
                }
            }
        }

        public async void ImportDataFromExcelCustomer(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        int id = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                        string name = worksheet.Cells[row, 2].Value.ToString();
                        string surName = worksheet.Cells[row, 3].Value.ToString();
                        string email = worksheet.Cells[row, 4].Value.ToString();

                        var customerDTO = new CustomerDTO
                        {
                            Id = id,
                            Name = name,
                            Surname = surName,
                            Email = email
                        };


                        var existingCustomer = await _customerRepository.GetCustomerById(id);

                        if (existingCustomer != null)
                        {
                            existingCustomer.Name = name;
                            existingCustomer.Surname = surName;
                            existingCustomer.Email = email;
                        }
                        else
                        {
                            var newCustomer = new Customer { Name = name, Surname = surName, Email = email };
                            _dataContext.Customers.Add(newCustomer);
                        }
                    }
                    _dataContext.SaveChanges();
                }
            }
        }

        void IImportDataExcel.ImportDataFromExcel(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        int id = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                        string street = worksheet.Cells[row, 2].Value.ToString();
                        string city = worksheet.Cells[row, 3].Value.ToString();
                        string state = worksheet.Cells[row, 4].Value.ToString();

                        var addressDto = new AddressDTO
                        {
                            Id = id,
                            Street = street,
                            City = city,
                            State = state
                        };


                        var existingAddress = _addressRepository.GetAddress(id);

                        if (existingAddress != null)
                        {
                            existingAddress.Street = street;
                            existingAddress.City = city;
                            existingAddress.State = state;
                        }
                        else
                        {
                            var newAddress = new Address { Street = street, City = city, State = state };
                            _dataContext.Addresses.Add(newAddress);
                        }
                    }
                    _dataContext.SaveChanges();
                }
            }
        }

        public async void ImportDataFromExcelOrder(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        int id = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                        DateTime date = DateTime.Parse(worksheet.Cells[row, 2].Text);

                        var orderDTO = new OrderDTO
                        {
                            Id = id,
                            DateTime = date,
                        };


                        var existingOrder = await _orderRepository.GetOrderById(id);

                        if (existingOrder != null)
                        {
                            existingOrder.DateTime = date;
                        }
                        else
                        {
                            var newOrder = new Order { DateTime = date };
                            _dataContext.Orders.Add(newOrder);
                        }
                    }
                    _dataContext.SaveChanges();
                }
            }
        }

        public async void ImportDataFromExcelProduct(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        int id = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                        string name = worksheet.Cells[row, 2].Value.ToString();
                        int  price = int.Parse(worksheet.Cells[row, 3].Value.ToString());

                        var productDTO = new ProductDTO
                        {
                            Id = id,
                            Name = name,
                            Price = price
                        };


                        var existingProduct = await _productRepository.GetProductById(id);

                        if (existingProduct != null)
                        {
                            existingProduct.Name = name;
                            existingProduct.Price = price;
                        }
                        else
                        {
                            var newProduct = new Product { Name = name, Price = price };
                            _dataContext.Products.Add(newProduct);
                        }
                    }
                    _dataContext.SaveChanges();
                }
            }
        }

        public async void ImportDataFromExcelUser(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        Guid id = Guid.Parse(worksheet.Cells[row, 1].Text);
                        byte[] imageProfile = Convert.FromBase64String(worksheet.Cells[row, 2].Value.ToString());
                        string userName = worksheet.Cells[row, 3].Value.ToString();

                        var userDTO = new UserDTO
                        {
                            Id = id.ToString(),
                            ImageProfile = imageProfile,
                            UserName = userName
                        };


                        var existingUser = await _userRepository.GetUserById(id);

                        if (existingUser != null)
                        {
                            existingUser.ImageProfile = imageProfile;
                            existingUser.UserName = userName;
                        }
                        else
                        {
                            var newUser = new User { ImageProfile = imageProfile, UserName = userName };
                            _dataContext.Users.Add(newUser);
                        }
                    }
                    _dataContext.SaveChanges();
                }
            }
        }

        public void ImportDataExcelUpdateAddress(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        int id = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                        string street = worksheet.Cells[row, 2].Value.ToString();
                        string city = worksheet.Cells[row, 3].Value.ToString();
                        string state = worksheet.Cells[row, 4].Value.ToString();

                        var existingAddress = _dataContext.Addresses.FirstOrDefault(a => a.Id == id);

                        if (existingAddress != null)
                        {
                            existingAddress.Street = street;
                            existingAddress.City = city;
                            existingAddress.State = state;
                        }
                    }
                    _dataContext.SaveChanges();
                }
            }
        }

        public async void ImportDataExcelUpdateCategoryProduct(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        int id = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                        string nameCategory = worksheet.Cells[row, 2].Value.ToString();

                        var categoryProductDTO = new CategoryProductDTO
                        {
                            Id = id,
                            NameCategory = nameCategory
                        };


                        var existingCategoryProduct = _dataContext.CategoryProducts.FirstOrDefault(a => a.Id == id);

                        if (existingCategoryProduct != null)
                        {
                            existingCategoryProduct.NameCategory = nameCategory;
                        }
                    }
                    _dataContext.SaveChanges();
                }
            }
        }

        public void ImportDataExcelUpdateCustomer(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        int id = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                        string name = worksheet.Cells[row, 2].Value.ToString();
                        string surName = worksheet.Cells[row, 3].Value.ToString();
                        string email = worksheet.Cells[row, 4].Value.ToString();

                        var customerDTO = new CustomerDTO
                        {
                            Id = id,
                            Name = name,
                            Surname = surName,
                            Email = email
                        };


                        var existingCustomer = _dataContext.Customers.FirstOrDefault(a => a.Id == id);

                        if (existingCustomer != null)
                        {
                            existingCustomer.Name = name;
                            existingCustomer.Surname = surName;
                            existingCustomer.Email = email;
                        }
                    }
                    _dataContext.SaveChanges();
                }
            }
        }

        public void ImportDataExcelUpdateOrder(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        int id = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                        DateTime date = DateTime.Parse(worksheet.Cells[row, 2].Text);

                        var orderDTO = new OrderDTO
                        {
                            Id = id,
                            DateTime = date,
                        };


                        var existingOrder = _dataContext.Orders.FirstOrDefault(a => a.Id == id);

                        if (existingOrder != null)
                        {
                            existingOrder.DateTime = date;
                        }
                    }
                    _dataContext.SaveChanges();
                }
            }
        }

        public void ImportDataExcelUpdateProduct(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        int id = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                        string name = worksheet.Cells[row, 2].Value.ToString();
                        int price = int.Parse(worksheet.Cells[row, 3].Value.ToString());

                        var productDTO = new ProductDTO
                        {
                            Id = id,
                            Name = name,
                            Price = price
                        };


                        var existingProduct = _dataContext.Products.FirstOrDefault(a => a.Id == id);

                        if (existingProduct != null)
                        {
                            existingProduct.Name = name;
                            existingProduct.Price = price;
                        }
                    }
                    _dataContext.SaveChanges();
                }
            }
        }

        public void ImportDataExcelUpdateUser(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        Guid id = Guid.Parse(worksheet.Cells[row, 1].Text);
                        byte[] imageProfile = Convert.FromBase64String(worksheet.Cells[row, 2].Value.ToString());
                        string userName = worksheet.Cells[row, 3].Value.ToString();

                        var userDTO = new UserDTO
                        {
                            Id = id.ToString(),
                            ImageProfile = imageProfile,
                            UserName = userName
                        };


                        var existingUser = _dataContext.Users.FirstOrDefault(a => a.Id == id.ToString());

                        if (existingUser != null)
                        {
                            existingUser.ImageProfile = imageProfile;
                            existingUser.UserName = userName;
                        }
                    }
                    _dataContext.SaveChanges();
                }
            }
        }
    }
}
