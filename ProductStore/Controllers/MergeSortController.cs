using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Core.Interface;
using ProductStore.DTO;
using ProductStore.Interface;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MergeSortController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryProductRepository _categoryProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISorting _sorting;
        public MergeSortController(IAddressRepository addressRepository, IOrderRepository orderRepository, ICategoryProductRepository categoryProductRepository, IProductRepository productRepository, IUserRepository userRepository, ICustomerRepository customerRepository, ISorting sorting)
        {
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _categoryProductRepository = categoryProductRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _sorting = sorting;
        }

        [HttpGet("MergeSortProduct")]
        public async Task<IActionResult> MergeSortProduct(string sortBy)
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

                var sortedProduct = _sorting.MergeSort(product.AsQueryable(), sortBy);
                return Ok(sortedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("MergeSortUser")]
        public async Task<IActionResult> MergeSortUser(string sortBy)
        {
            try
            {
                var userDtos = await _userRepository.GetUsers();

                var user = userDtos.Select(dto => new UserDTO
                {
                    Id = dto.Id,
                    ImageProfile = dto.ImageProfile,
                    ConfirmPassword = dto.ConfirmPassword,
                    Email = dto.Email,
                    Password = dto.Password,
                    UserName = dto.UserName,
                });

                var sortedUser = _sorting.MergeSort(user.AsQueryable(), sortBy);
                return Ok(sortedUser);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("MergeSortAddress")]
        public async Task<IActionResult> MergeSortAddress(string sortBy)
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

                var sortedAddress = _sorting.MergeSort(address.AsQueryable(), sortBy);
                return Ok(sortedAddress);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("MergeSortCategoryProduct")]
        public async Task<IActionResult> MergeSortCategoryProduct(string sortBy)
        {
            try
            {
                var productCategoryDtos = await _categoryProductRepository.GetCategoryProducts();

                var productCategory = productCategoryDtos.Select(dto => new CategoryProductDTO
                {
                    Id = dto.Id,
                    NameCategory = dto.NameCategory
                });

                var sortedProductCategory = _sorting.MergeSort(productCategory.AsQueryable(), sortBy);
                return Ok(sortedProductCategory);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("MergeSortCustomer")]
        public async Task<IActionResult> MergeSortCustomer(string sortBy)
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

                var sortedCustomer = _sorting.MergeSort(customer.AsQueryable(), sortBy);
                return Ok(sortedCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("MergeSortOrder")]
        public async Task<IActionResult> MergeSortOrder(string sortBy)
        {
            try
            {
                var orderDtos = await _orderRepository.GetOrders();

                var order = orderDtos.Select(dto => new OrderDTO
                {
                    Id = dto.Id,
                    DateTime = dto.DateTime,
                });

                var sortedOrder = _sorting.MergeSort(order.AsQueryable(), sortBy);
                return Ok(sortedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
