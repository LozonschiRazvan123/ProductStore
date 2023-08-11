using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                return BadRequest(ModelState);
            }

            return Ok(await _customerRepository.GetCustomers());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _customerRepository.GetCustomerById(id));
        }


        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerCreateDTO)
        {
            if (customerCreateDTO == null)
            {
                return BadRequest(ModelState);
            }

            var customer = _customerRepository.Add(customerCreateDTO);

            if (customer != null)
            {
                ModelState.AddModelError("", "Customer already exists!");
            }

            if(!customer)
            {
                ModelState.AddModelError("", "Something went wrong!");
            }

            return Ok("Successfully created!");
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(int customerId, [FromBody] CustomerDTO updateCustomer)
        {
            if (updateCustomer == null || customerId != updateCustomer.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!_customerRepository.Update(updateCustomer))
            {
                ModelState.AddModelError("", "Something is wrong!");
            }

            return Ok("Successfully update!");
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteAddress(int customerId)
        {
            if (!_customerRepository.ExistCostumer(customerId))
            {
                return NotFound();
            }

            var customerDelete = await _customerRepository.GetCustomerById(customerId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_customerRepository.DeleteCustomer(customerDelete))
            {
                ModelState.AddModelError("", "Something is wrong!");
            }

            return NoContent();
        }
    }
}
