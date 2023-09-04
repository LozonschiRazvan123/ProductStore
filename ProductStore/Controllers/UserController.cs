using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductStore.ConfigurationError;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Migrations;
using ProductStore.Models;
using ProductStore.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
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

            bool userCreate = _userRepository.Add(user);
            if(userCreate == false)
            {
                throw new ExistModel("User");
            }

            if(!userCreate)
            {
                throw new BadRequest();
            }
            return Ok("Successfully create");
        }

        [HttpPost("AddImageProfile/{userId}")]
        public async Task<IActionResult> AddImageProfile(int userId, [FromForm] IFormFile image)
        {
            var userTask = _userRepository.GetUserById(userId);
            var user = await userTask;

            if (user == null)
            {
                return NotFound();
            }

            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    user.ImageProfile = memoryStream.ToArray();

                    bool updateSuccessful = _userRepository.Update(user);
                    if (updateSuccessful)
                    {
                        return Ok("Image profile added successfully.");
                    }
                    else
                    {
                        return BadRequest("Failed to update user with image.");
                    }
                }
            }

            return BadRequest("Invalid image.");
        }

        [HttpPut("UpdateImageProfile/{userId}")]
        public async Task<IActionResult> UpdateImageProfile(int userId, [FromForm] IFormFile image)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    user.ImageProfile = memoryStream.ToArray();

                    bool updateSuccessful = _userRepository.Update(user);
                    if (updateSuccessful)
                    {
                        return Ok("Image profile updated successfully.");
                    }
                    else
                    {
                        return BadRequest("Failed to update user's image.");
                    }
                }
            }

            return BadRequest("Invalid image.");
        }


        [HttpDelete("DeleteImageProfile/{userId}")]
        public async Task<IActionResult> DeleteImageProfile(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.ImageProfile = null; 

            bool updateSuccessful = _userRepository.Update(user);
            if (updateSuccessful)
            {
                return Ok("Image profile deleted successfully.");
            }
            else
            {
                return BadRequest("Failed to delete user's image.");
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private string CreateRandomToken(bool isAdmin = false)
        {
            List<Claim> claims = new List<Claim>();

            if (isAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var a = _configuration.GetSection("AppSettings:Token").Value;
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(a));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRegister request)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (!VerifiedPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password is incorrect");
            }

            if (user.VerifiedAt == null)
            {
                return BadRequest("Not veriffied!");
            }

            return Ok($"Welcome back, {user.Email}!");

        }


        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            var user = await _userRepository.GetUserByToken(token);
            if (user == null)
            {
                return BadRequest("Invalid token");
            }

            user.VerifiedAt = DateTime.Now;
            _userRepository.Save();

            return Ok("User verified!");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return BadRequest("User not found!");
            }

            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            _userRepository.Save();

            return Ok("You may now reset your password");

        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> RessetPassword(ResetPassword request)
        {
            var user = await _userRepository.GetUserByToken(request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("User not found!");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
            _userRepository.Save();

            return Ok("Password successfully reset");

        }

        private bool VerifiedPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }


        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
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
