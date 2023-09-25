using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.ConfigurationError;
using ProductStore.DTO;
using ProductStore.Framework.Pagination;
using ProductStore.Interface;
using ProductStore.Models;
namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository) 
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            return Ok(await _customerRepository.GetCustomers());
        }

        [HttpGet("api/CustomerPagination")]
        public async Task<IActionResult> GetCustomerPagination([FromQuery] PaginationFilter filter)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            var customers = await _customerRepository.GetCustomersPagination(filter);

            var response = new
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = customers.TotalPages,
                TotalRecords = customers.TotalRecords,
                Customers = customers.Results
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }
            return Ok(await _customerRepository.GetCustomerById(id));
        }


        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerCreateDTO)
        {
            if (customerCreateDTO == null)
            {
                throw new BadRequest();
            }

            var customer = _customerRepository.Add(customerCreateDTO);

            if (customer == false)
            {
                throw new ExistModel("Customer");
            }

            if(!customer)
            {
                throw new BadRequest();
            }

            return Ok("Successfully created!");
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(int customerId, [FromBody] CustomerDTO updateCustomer)
        {
            if (updateCustomer == null || customerId != updateCustomer.Id)
            {
                throw new BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!_customerRepository.Update(updateCustomer))
            {
                throw new BadRequest();
            }

            return Ok("Successfully update!");
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            if (!_customerRepository.ExistCostumer(customerId))
            {
                throw new AppException("Customer", customerId.ToString());
            }

            var customerDelete = await _customerRepository.GetCustomerById(customerId);
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            if (!_customerRepository.DeleteCustomer(customerDelete))
            {
                throw new BadRequest();
            }

            return NoContent();
        }
    }
}
