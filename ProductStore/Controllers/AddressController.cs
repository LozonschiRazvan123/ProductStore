using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.ConfigurationError;
using ProductStore.Core.Interface;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Framework.Pagination;
using ProductStore.Interface;
using ProductStore.Models;
using System.Reflection.Metadata.Ecma335;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IServicePagination<Address> _servicePagination;
        private readonly DataContext _dataContext;
        public AddressController(IAddressRepository addressRepository, IServicePagination<Address> servicePagination, DataContext dataContext)
        {
            _addressRepository = addressRepository;
            _servicePagination = servicePagination;
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult GetAddress() 
        {
            return Ok(_addressRepository.GetAddresses());
        }

        [HttpGet("{id}")]
        public IActionResult GetAddressById(int id)
        {
            if(_addressRepository.GetAddress(id) == null)
            {
                throw new AppException("Address", id.ToString());
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_addressRepository.GetAddress(id));
        }

        [HttpGet("api/CustomerPagination")]
        public async Task<IActionResult> GetCustomerPagination([FromQuery] PaginationFilter filter)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            //IQueryable<Customer> query = _dataContext.Customers;
            var customers = await _servicePagination.GetCustomersPagination(_dataContext.Addresses, filter);

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

        [HttpPost]
        public IActionResult CreateAddress([FromBody] AddressDTO addressCreateDTO)
        {
            if(addressCreateDTO == null)
            {
                return BadRequest(ModelState);
            }

            var address = _addressRepository.CreateAddress(addressCreateDTO);

            if(address == false)
            {
                throw new ExistModel("Address");
            }


            if(!address)
            {
                throw new BadRequest();
            }

            return Ok("Successfully created!");
        }

        [HttpPut("{addressId}")]
        public IActionResult UpdateAddress(int addressId, [FromBody] AddressDTO updateAddress)
        {
            if (updateAddress == null || addressId != updateAddress.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(!_addressRepository.UpdateAddress(updateAddress))
            {
                throw new BadRequest();
            }

            return Ok("Successfully update!");
        }

        [HttpDelete("{addressId}")]
        public IActionResult DeleteAddress(int addressId)
        {
            if(!_addressRepository.AddressExist(addressId))
            {
                throw new AppException("Address", addressId.ToString());
            }

            var addressDelete = _addressRepository.GetAddress(addressId);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!_addressRepository.DeleteAddress(addressDelete))
            {
                throw new BadRequest();
            }

            return NoContent();
        }

    }
}
