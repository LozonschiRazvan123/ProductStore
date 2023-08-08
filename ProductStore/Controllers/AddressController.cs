using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var addresses = _addressRepository.GetAddresses();

            var addressesDTO = addresses.Select(Address => new AddressDTO
            {
                Id = Address.Id,
                City = Address.City,
                State = Address.State,
                Street = Address.Street
            }).ToList();
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(addressesDTO);

        }

        [HttpGet("{id}")]
        public IActionResult GetAddressById(int id)
        {
            var address = _addressRepository.GetAddress(id);

            if(address == null)
            {
                return NotFound();
            }

            var addressDTO = new AddressDTO
            { 
                Id = address.Id,
                City = address.City,
                State = address.State,
                Street = address.Street
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(addressDTO);
        }

        [HttpPost]
        public IActionResult CreateAddress([FromBody] AddressDTO addressCreateDTO)
        {
            if(addressCreateDTO == null)
            {
                return BadRequest(ModelState);
            }

            var address = _addressRepository.GetAddresses().Where(a => a.Street == addressCreateDTO.Street).FirstOrDefault();

            if(address != null)
            {
                ModelState.AddModelError("", "Address is already exists");
            }

            var addressCreate = new Address
            {
                Id = addressCreateDTO.Id,
                City = addressCreateDTO.City,
                State = addressCreateDTO.State,
                Street = addressCreateDTO.Street
            };

            if(!_addressRepository.CreateAddress(addressCreate))
            {
                ModelState.AddModelError("", "Something is wrong!");
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

            var updateAddressDTO = new Address
            {
                Id = updateAddress.Id,
                City = updateAddress.City,
                State = updateAddress.State,
                Street = updateAddress.Street
            };

            if(!_addressRepository.UpdateAddress(updateAddressDTO))
            {
                ModelState.AddModelError("", "Something is wrong!");
            }

            return Ok("Successfully update!");
        }

        [HttpDelete("{addressId}")]
        public IActionResult DeleteAddress(int addressId)
        {
            if(!_addressRepository.AddressExist(addressId))
            {
                return NotFound();
            }

            var addressDelete = _addressRepository.GetAddress(addressId);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!_addressRepository.DeleteAddress(addressDelete))
            {
                ModelState.AddModelError("", "Something is wrong!");
            }

            return NoContent();
        }

    }
}
