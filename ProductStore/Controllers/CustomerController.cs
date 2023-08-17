using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.ConfigurationError;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;
using ProductStore.Repository;

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

            if (customer != null)
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
        public async Task<IActionResult> DeleteAddress(int customerId)
        {
            if (!_customerRepository.ExistCostumer(customerId))
            {
                throw new AppException("Customer", customerId);
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
