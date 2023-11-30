using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Core.Interface;
using ProductStore.DTO;
using ProductStore.Interface;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsertionSortController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryProductRepository _categoryProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISorting _sorting;
        public InsertionSortController(IAddressRepository addressRepository, IOrderRepository orderRepository, ICategoryProductRepository categoryProductRepository, IProductRepository productRepository, IUserRepository userRepository, ICustomerRepository customerRepository, ISorting sorting)
        {
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _categoryProductRepository = categoryProductRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _sorting = sorting;
        }

        [HttpGet("insertionSortProduct")]
        public async Task<IActionResult> InsertionSortProduct(string sortBy)
        {
            try
            {
                var productDtos = await _productRepository.GetProducts();

                var product = productDtos.Select(dto => new ProductDTO
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price,
                    Street = dto.Street
                }).AsQueryable();

                var sortedProduct = _sorting.InsertionSort(product, sortBy);
                return Ok(sortedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("insertionSortAddress")]
        public async Task<IActionResult> InsertionSortAddress(string sortBy)
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
                }).AsQueryable();

                var sortedAddress = _sorting.InsertionSort(address, sortBy);
                return Ok(sortedAddress);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("insertionSortCategoryProduct")]
        public async Task<IActionResult> InsertionSortCategoryProduct(string sortBy)
        {
            try
            {
                var categoryProductDtos = await _categoryProductRepository.GetCategoryProducts();

                var categoryProduct = categoryProductDtos.Select(dto => new CategoryProductDTO
                {
                    Id = dto.Id,
                    NameCategory = dto.NameCategory
                }).AsQueryable();

                var sortedCategoryProduct = _sorting.InsertionSort(categoryProduct, sortBy);
                return Ok(sortedCategoryProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("insertionSortCustomer")]
        public async Task<IActionResult> InsertionSortCustomer(string sortBy)
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
                }).AsQueryable();

                var sortedCustomer = _sorting.InsertionSort(customer, sortBy);
                return Ok(sortedCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("insertionSortOrder")]
        public async Task<IActionResult> InsertionSortOrder(string sortBy)
        {
            try
            {
                var orderDtos = await _orderRepository.GetOrders();

                var order = orderDtos.Select(dto => new OrderDTO
                {
                    Id = dto.Id,
                    DateTime = dto.DateTime,
                }).AsQueryable();

                var sortedOrder = _sorting.InsertionSort(order, sortBy);
                return Ok(sortedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("insertionSortUser")]
        public async Task<IActionResult> InsertionSortUser(string sortBy)
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
                }).AsQueryable();

                var sortedUser = _sorting.InsertionSort(user, sortBy);
                return Ok(sortedUser);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

    }
}
