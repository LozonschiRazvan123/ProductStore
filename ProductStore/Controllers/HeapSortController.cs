using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Core.Interface;
using ProductStore.DTO;
using ProductStore.Interface;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeapSortController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryProductRepository _categoryProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISorting _sorting;
        public HeapSortController(IAddressRepository addressRepository, IOrderRepository orderRepository, ICategoryProductRepository categoryProductRepository, IProductRepository productRepository, IUserRepository userRepository, ICustomerRepository customerRepository, ISorting sorting)
        {
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _categoryProductRepository = categoryProductRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _sorting = sorting;
        }

        [HttpGet("HeapSortProduct")]
        public async Task<IActionResult> HeapSortProduct(string sortBy)
        {
            try
            {
                var productDtos = await _productRepository.GetProducts();

                var product = productDtos.Select(dto => new ProductDTO
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price,
                    Street = dto.Street,
                });

                var sortedProduct = _sorting.HeapSort(product.AsQueryable(), sortBy);
                return Ok(sortedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("HeapSortUser")]
        public async Task<IActionResult> HeapSortUser(string sortBy)
        {
            try
            {
                var userDtos = await _userRepository.GetUsers();

                var user = userDtos.Select(dto => new UserDTO
                {
                    Id = dto.Id,
                    UserName = dto.UserName,
                    ConfirmPassword = dto.ConfirmPassword,
                    Email = dto.Email,
                    ImageProfile = dto.ImageProfile,
                    Password = dto.Password
                });

                var sortedUser = _sorting.HeapSort(user.AsQueryable(), sortBy);
                return Ok(sortedUser);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("HeapSortAddress")]
        public async Task<IActionResult> HeapSortAddress(string sortBy)
        {
            try
            {
                var addressDtos = _addressRepository.GetAddresses();

                var address = addressDtos.Select(dto => new AddressDTO
                {
                    Id = dto.Id,
                    Street = dto.Street,
                    City = dto.City,
                    State = dto.State,
                });

                var sortedAddress = _sorting.HeapSort(address.AsQueryable(), sortBy);
                return Ok(sortedAddress);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("HeapSortCategoryProduct")]
        public async Task<IActionResult> HeapSortCategoryProduct(string sortBy)
        {
            try
            {
                var categoryProductDtos = await _categoryProductRepository.GetCategoryProducts();

                var categoryProduct = categoryProductDtos.Select(dto => new CategoryProductDTO
                {
                    Id = dto.Id,
                    NameCategory = dto.NameCategory,
                });

                var sortedCategoryProduct = _sorting.HeapSort(categoryProduct.AsQueryable(), sortBy);
                return Ok(sortedCategoryProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("HeapSortCustomer")]
        public async Task<IActionResult> HeapSortCustomer(string sortBy)
        {
            try
            {
                var customerDtos = await _customerRepository.GetCustomers();

                var customer = customerDtos.Select(dto => new CustomerDTO
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Email = dto.Email,
                    Surname = dto.Surname,
                });

                var sortedCustomer = _sorting.HeapSort(customer.AsQueryable(), sortBy);
                return Ok(sortedCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("HeapSortOrder")]
        public async Task<IActionResult> HeapSortOrder(string sortBy)
        {
            try
            {
                var orderDtos = await _orderRepository.GetOrders();

                var order = orderDtos.Select(dto => new OrderDTO
                {
                    Id = dto.Id,
                    DateTime = dto.DateTime,
                });

                var sortedOrder = _sorting.HeapSort(order.AsQueryable(), sortBy);
                return Ok(sortedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
