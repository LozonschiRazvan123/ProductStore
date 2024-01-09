using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.ConfigurationError;
using ProductStore.Core.Interface;
using ProductStore.DTO;
using ProductStore.Models;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerControllerDapper : ControllerBase
    {
        private readonly ICustomerRepositoryDapper _customerRepositoryDapper;
        public CustomerControllerDapper(ICustomerRepositoryDapper customerRepositoryDapper)
        {
            _customerRepositoryDapper = customerRepositoryDapper;
        }

        [HttpGet("customersDapper")]
        public async Task<IActionResult> GetCustomerDapper()
        {
            var customers = await _customerRepositoryDapper.GetCustomers();
            return Ok(customers);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerDapperId(int customerId)
        {
            var customer = await _customerRepositoryDapper.GetCustomersId(customerId);
            return Ok(customer);
        }

        [HttpPut("customerUpdate/{customerId}")]
        public async Task<IActionResult> UpdateCustomer(int customerId, CustomerDTO customer)
        {
            if (await _customerRepositoryDapper.UpdateCustomer(customerId, customer) == null)
            {
                throw new BadRequest();
            }
            return Ok("Successfully update!");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerDTO customer)
        {
            if(await _customerRepositoryDapper.CreateCustomer(customer) == null)
            {
                throw new BadRequest();
            }
            return Ok("Successfully created");
        }

        [HttpDelete("customers/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            try
            {
                await _customerRepositoryDapper.DeleteCustomer(customerId);
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting customer: {ex.Message}");
            }
        }


    }
}
