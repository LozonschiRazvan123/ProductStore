using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Core.Interface;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SortingController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryProductRepository _categoryProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISorting _sorting;
        public SortingController(IAddressRepository addressRepository, IOrderRepository orderRepository, ICategoryProductRepository categoryProductRepository, IProductRepository productRepository, IUserRepository userRepository, ICustomerRepository customerRepository, ISorting sorting)
        {
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _categoryProductRepository = categoryProductRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _sorting = sorting;
        }

        [HttpGet("sortedAddressShell")]
        public async Task<IActionResult> GetSortedAddress(string sortBy)
        {
            try
            {
                var addressDtos = _addressRepository.GetAddresses();

                var address = addressDtos.Select(dto => new AddressDTO
                {
                    Id = dto.Id,
                    City = dto.City,
                    State = dto.State,
                    Street = dto.Street,
                });

                var sortedAddress = _sorting.ApplyShellSort(address.AsQueryable(), sortBy);
                return Ok(sortedAddress);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("sortedCategoryProductShell")]
        public async Task<IActionResult> GetSortedCategoryProduct(string sortBy)
        {
            try
            {
                var productCategoryDtos = await _categoryProductRepository.GetCategoryProducts();

                var productsCategory = productCategoryDtos.Select(dto => new CategoryProductDTO
                {
                    Id = dto.Id,
                    NameCategory = dto.NameCategory
                });

                var sortedProductsCategory = _sorting.ApplyShellSort(productsCategory.AsQueryable(), sortBy);
                return Ok(sortedProductsCategory);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("sortedCustomerShell")]
        public async Task<IActionResult> GetSortedCustomer(string sortBy)
        {
            try
            {
                var CustomerDtos = await _customerRepository.GetCustomers();

                var customer = CustomerDtos.Select(dto => new CustomerDTO
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Email = dto.Email,
                });

                var sortedCustomers = _sorting.ApplyShellSort(customer.AsQueryable(), sortBy);
                return Ok(sortedCustomers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("sortedOrderShell")]
        public async Task<IActionResult> GetSortedOrder(string sortBy)
        {
            try
            {
                var orderDtos = await _orderRepository.GetOrders();

                var orders = orderDtos.Select(dto => new OrderDTO
                {
                    Id = dto.Id,
                    DateTime = dto.DateTime,
                });

                var sortedOrders = _sorting.ApplyShellSort(orders.AsQueryable(), sortBy);
                return Ok(sortedOrders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpGet("sortedProductShell")]
        public async Task<IActionResult> GetSortedProducts(string sortBy)
        {
            try
            {
                var productDtos = await _productRepository.GetProducts();

                var products = productDtos.Select(dto => new ProductDTO
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price
                });

                var sortedProducts = _sorting.ApplyShellSort(products.AsQueryable(), sortBy);
                return Ok(sortedProducts);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("sortedUserShell")]
        public async Task<IActionResult> GetSortedUser(string sortBy)
        {
            try
            {
                var userDtos = await _userRepository.GetUsers();

                var users = userDtos.Select(dto => new UserDTO
                {
                    Id = dto.Id,
                    UserName = dto.UserName,
                    Email = dto.Email,
                });

                var sortedUser = _sorting.ApplyShellSort(users.AsQueryable(), sortBy);
                return Ok(sortedUser);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("getSortedProducts")]
        public async Task<IActionResult> GetSortedProducts1(string sortBy)
        {
            try
            {
                var userDtos = await _userRepository.GetUsers();

                var users = userDtos.Select(dto => new UserDTO
                {
                    Id = dto.Id,
                    UserName = dto.UserName,
                    Email = dto.Email,
                    //Age = dto.Age // Presupunând că vrei să sortezi după vârstă
                });

                var sortedUsers = ApplyCountingSort(users.AsQueryable(), sortBy);
                return Ok(sortedUsers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        public IQueryable<T> ApplyCountingSort<T>(IQueryable<T> query, string sortBy)
        {
            var list = query.ToList();
            CountingSort(list, sortBy);

            return list.AsQueryable();
        }


        private void CountingSort<T>(List<T> list, string sortBy)
        {
            PropertyInfo prop = typeof(T).GetProperty(sortBy);

            int n = list.Count;

            var comparer = Comparer<T>.Default;

            // Construiește array-ul de frecvențe
            SortedDictionary<T, int> count = new SortedDictionary<T, int>(comparer);
            foreach (var item in list)
            {
                // Verifică dacă obiectul din listă este null
                if (item == null)
                {
                    throw new ArgumentNullException("One or more objects in the list are null.");
                }

                // Verifică dacă tipul obiectului din listă este compatibil cu T
                if (item is T typedItem)
                {
                    T value = (T)prop.GetValue(typedItem);

                    if (count.ContainsKey(value))
                        count[value]++;
                    else
                        count[value] = 1;
                }
                else
                {
                    throw new ArgumentException($"Object in the list is not of type '{typeof(T).Name}'.");
                }
            }


            // Reconstruiește lista sortată
            int index = 0;
            foreach (var key in count.Keys)
            {
                int frequency = count[key];
                for (int i = 0; i < frequency; i++)
                {
                    prop.SetValue(list[index], key);
                    index++;
                }
            }
        }

    }
}
