using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Core.Interface;
using ProductStore.DTO;
using ProductStore.Interface;
using System.Reflection;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SortingCountingController : ControllerBase
    {

        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryProductRepository _categoryProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISorting _sorting;
        public SortingCountingController(IAddressRepository addressRepository, IOrderRepository orderRepository, ICategoryProductRepository categoryProductRepository, IProductRepository productRepository, IUserRepository userRepository, ICustomerRepository customerRepository, ISorting sorting)
        {
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _categoryProductRepository = categoryProductRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _sorting = sorting;
        }
        [HttpGet("getSortedUser")]
        public async Task<IActionResult> GetSorteUser(string sortBy)
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

                var sortedUsers = _sorting.ApplyCountingSort(users.AsQueryable(), sortBy);
                return Ok(sortedUsers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpGet("getSortedAddress")]
        public async Task<IActionResult> GetSortedAddress(string sortBy)
        {
            try
            {
                var addressDtos = _addressRepository.GetAddresses();

                var adresses = addressDtos.Select(dto => new AddressDTO
                {
                    Id = dto.Id,
                    City = dto.City,
                    State = dto.State,
                    Street = dto.Street,
                });

                var sortedAddress = _sorting.ApplyCountingSort(adresses.AsQueryable(), sortBy);
                return Ok(sortedAddress);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpGet("getSortedCategoryProduct")]
        public async Task<IActionResult> GetSortedCategoryProduct(string sortBy)
        {
            try
            {
                var categoryProductDtos = await _categoryProductRepository.GetCategoryProducts();

                var categoryProducts = categoryProductDtos.Select(dto => new CategoryProductDTO
                {
                    Id = dto.Id,
                    NameCategory = dto.NameCategory
                });

                var sortedCategoryProduct = _sorting.ApplyCountingSort(categoryProducts.AsQueryable(), sortBy);
                return Ok(sortedCategoryProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpGet("getSortedCustomer")]
        public async Task<IActionResult> GetSortedCustomer(string sortBy)
        {
            try
            {
                var customerDtos = await _customerRepository.GetCustomers();

                var customers = customerDtos.Select(dto => new CustomerDTO
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Email = dto.Email
                });

                var sortedCustomer = _sorting.ApplyCountingSort(customers.AsQueryable(), sortBy);
                return Ok(sortedCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("getSortedOrder")]
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

                var sortedOrder = _sorting.ApplyCountingSort(orders.AsQueryable(), sortBy);
                return Ok(sortedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("getSortedProduct")]
        public async Task<IActionResult> GetSortedProduct(string sortBy)
        {
            try
            {
                var productDtos = await _productRepository.GetProducts();

                var products = productDtos.Select(dto => new ProductDTO
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price,
                    Street = dto.Street,
                });

                var sortedProduct = _sorting.ApplyCountingSort(products.AsQueryable(), sortBy);
                return Ok(sortedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

    }
}
