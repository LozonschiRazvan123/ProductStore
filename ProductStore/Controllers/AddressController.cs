using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.ConfigurationError;
using ProductStore.DTO;
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
        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
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
                throw new AppException("Address", id);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_addressRepository.GetAddress(id));
        }

        [HttpPost]
        public IActionResult CreateAddress([FromBody] AddressDTO addressCreateDTO)
        {
            if(addressCreateDTO == null)
            {
                return BadRequest(ModelState);
            }

            var address = _addressRepository.CreateAddress(addressCreateDTO);

            if(address != null)
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
                throw new AppException("Address", addressId);
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
