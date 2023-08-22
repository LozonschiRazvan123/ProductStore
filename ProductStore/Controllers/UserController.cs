using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.ConfigurationError;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;
using ProductStore.Repository;
using System.Security.Cryptography;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository) 
        {
            _userRepository = userRepository; 
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }
            return Ok(await _userRepository.GetUsers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }
            return Ok(await _userRepository.GetUserById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            if(userDTO == null)
            {
                throw new BadRequest();
            }
            CreatePasswordHash(userDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Email = userDTO.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerificationToken = CreateRandomToken(),
                Password = userDTO.Password,
                UserName = userDTO.UserName
            };

            var userCreate = _userRepository.Add(user);
/*            if(userCreate != null )
            {
                throw new ExistModel("User");
            }*/

            if(!userCreate)
            {
                throw new BadRequest();
            }
            return Ok("Successfully create");
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!_userRepository.ExistUser(id))
            {
                throw new AppException("User", id);
            }
            var userDelete = await _userRepository.GetUserById(id);
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            if (!_userRepository.Delete(userDelete))
            {
                throw new BadRequest();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDTOUpdate, int id)
        {
            if (userDTOUpdate == null || userDTOUpdate.Id != id)
            {
                throw new BadRequest();
            }

            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }
            if (!_userRepository.Update(userDTOUpdate))
            {
                throw new BadRequest();
            }
            return Ok("Update successfully!");
        }
    }
}
