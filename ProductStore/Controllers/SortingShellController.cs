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
    public class SortingShellController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryProductRepository _categoryProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISorting _sorting;
        public SortingShellController(IAddressRepository addressRepository, IOrderRepository orderRepository, ICategoryProductRepository categoryProductRepository, IProductRepository productRepository, IUserRepository userRepository, ICustomerRepository customerRepository, ISorting sorting)
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
    }
}
