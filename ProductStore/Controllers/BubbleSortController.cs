using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Core.Interface;
using ProductStore.DTO;
using ProductStore.Interface;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BubbleSortController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryProductRepository _categoryProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISorting _sorting;
        public BubbleSortController(IAddressRepository addressRepository, IOrderRepository orderRepository, ICategoryProductRepository categoryProductRepository, IProductRepository productRepository, IUserRepository userRepository, ICustomerRepository customerRepository, ISorting sorting)
        {
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _categoryProductRepository = categoryProductRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _sorting = sorting;
        }

        [HttpGet("BubbleSortUser")]
        public async Task<IActionResult> BubbleSortUser(string sortBy)
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

                var sortedUsers = _sorting.BubleSort(users.AsQueryable(), sortBy);
                return Ok(sortedUsers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("BubbleSortProduct")]
        public async Task<IActionResult> BubbleSorProduct(string sortBy)
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

                var sortedProduct = _sorting.BubleSort(product.AsQueryable(), sortBy);
                return Ok(sortedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("BubbleSortAddress")]
        public async Task<IActionResult> BubbleSortAddress(string sortBy)
        {
            try
            {
                var addressDtos = _addressRepository.GetAddresses();

                var address = addressDtos.Select(dto => new AddressDTO
                {
                    Id = dto.Id,
                    City = dto.City,
                    State = dto.State,
                    Street = dto.Street
                });

                var sortedAddress = _sorting.BubleSort(address.AsQueryable(), sortBy);
                return Ok(sortedAddress);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("BubbleSortCategoryProduct")]
        public async Task<IActionResult> BubbleSortCategoryProduct(string sortBy)
        {
            try
            {
                var categoryProductDtos = await _categoryProductRepository.GetCategoryProducts();

                var categoryProduct = categoryProductDtos.Select(dto => new CategoryProductDTO
                {
                    Id = dto.Id,
                    NameCategory = dto.NameCategory,
                });

                var sortedCategoryProduct = _sorting.BubleSort(categoryProduct.AsQueryable(), sortBy);
                return Ok(sortedCategoryProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("BubbleSortCustomer")]
        public async Task<IActionResult> BubbleSortCustomer(string sortBy)
        {
            try
            {
                var customerDtos = await _customerRepository.GetCustomers();

                var customer = customerDtos.Select(dto => new CustomerDTO
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Email = dto.Email,
                    Surname = dto.Surname
                });

                var sortedCustomer = _sorting.BubleSort(customer.AsQueryable(), sortBy);
                return Ok(sortedCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("BubbleSortOrder")]
        public async Task<IActionResult> BubbleSortOrder(string sortBy)
        {
            try
            {
                var orderDtos = await _orderRepository.GetOrders();

                var order = orderDtos.Select(dto => new OrderDTO
                {
                    Id = dto.Id,
                    DateTime = dto.DateTime,
                });

                var sortedOrder = _sorting.BubleSort(order.AsQueryable(), sortBy);
                return Ok(sortedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

    }
}
