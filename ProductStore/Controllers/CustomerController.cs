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
            var customer = await _customerRepository.GetCustomers();

            var customerDTO = customer.Select(Customer => new CustomerDTO
            {
                Id = Customer.Id,
                Name = Customer.Name,
                Surname = Customer.Surname,
                Email = Customer.Email
            }).ToList();


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(customerDTO);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var costumer = await _customerRepository.GetCustomerById(id);

            if (costumer == null)
            {
                return NotFound();
            }

            var costumerDTO = new CustomerDTO
            {
                Id = costumer.Id,
                Name = costumer.Name,
                Surname = costumer.Surname,
                Email = costumer.Email
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(costumerDTO);
        }


        /*[HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerCreateDTO)
        {
            if (customerCreateDTO == null)
            {
                return BadRequest(ModelState);
            }

            var customer = _customerRepository.GetCustomers().Where(c => c.Email == customerCreateDTO.Email).FirstOrDefault();

            if (customer != null)
            {
                ModelState.AddModelError("", "Customer is already exists");
            }

            var customerCreate = new Customer
            {
                Id = customerCreateDTO.Id,
                Name = customerCreateDTO.Name,
                Surname = customerCreateDTO.Surname,
                Email = customerCreateDTO.Email
            };

            if (!_customerRepository.Add(customerCreate))
            {
                ModelState.AddModelError("", "Something is wrong!");
            }

            return Ok("Successfully created!");
        }*/

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

            var updateCustomerDTO = new Customer
            {
                Id = updateCustomer.Id,
                Name = updateCustomer.Name,
                Surname = updateCustomer.Surname,
                Email = updateCustomer.Email
            };

            if (!_customerRepository.Update(updateCustomerDTO))
            {
                ModelState.AddModelError("", "Something is wrong!");
            }

            return Ok("Successfully update!");
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            if (!_customerRepository.ExistCostumer(addressId))
            {
                return NotFound();
            }

            var customerDelete = await _customerRepository.GetCustomerById(addressId);
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
