using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Core.Interface;
using ProductStore.DTO;
using ProductStore.Interface;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectionSort : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryProductRepository _categoryProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISorting _sorting;
        public SelectionSort(IAddressRepository addressRepository, IOrderRepository orderRepository, ICategoryProductRepository categoryProductRepository, IProductRepository productRepository, IUserRepository userRepository, ICustomerRepository customerRepository, ISorting sorting)
        {
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _categoryProductRepository = categoryProductRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _sorting = sorting;
        }

        [HttpGet("SelectionSortProduct")]
        public async Task<IActionResult> SelectionSortProduct(string propertyName)
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

                var sortedProduct = _sorting.SelectionSort(product.AsQueryable(), propertyName);
                return Ok(sortedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SelectionSortUser")]
        public async Task<IActionResult> SelectionSortUser(string propertyName)
        {
            try
            {
                var userDtos = await _userRepository.GetUsers();
                var user = userDtos.Select(dto => new UserDTO
                {
                    Id = dto.Id,
                    Password = dto.Password,
                    ConfirmPassword = dto.ConfirmPassword,
                    ImageProfile = dto.ImageProfile,
                    Email = dto.Email,
                    UserName = dto.UserName,
                });

                var sortedUser = _sorting.SelectionSort(user.AsQueryable(), propertyName);
                return Ok(sortedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SelectionSortAddress")]
        public async Task<IActionResult> SelectionSortAddress(string propertyName)
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

                var sortedAddress = _sorting.SelectionSort(address.AsQueryable(), propertyName);
                return Ok(sortedAddress);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SelectionSortCategoryProduct")]
        public async Task<IActionResult> SelectionSortCategoryProduct(string propertyName)
        {
            try
            {
                var productCategoryDtos = await _categoryProductRepository.GetCategoryProducts();
                var productCategory = productCategoryDtos.Select(dto => new CategoryProductDTO
                {
                    Id = dto.Id,
                    NameCategory = dto.NameCategory,
                });

                var sortedProductCategory = _sorting.SelectionSort(productCategory.AsQueryable(), propertyName);
                return Ok(sortedProductCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SelectionSortCustomer")]
        public async Task<IActionResult> SelectionSortCustomer(string propertyName)
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

                var sortedCustomer = _sorting.SelectionSort(customer.AsQueryable(), propertyName);
                return Ok(sortedCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SelectionSortOrder")]
        public async Task<IActionResult> SelectionSortOrder(string sortBy)
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
