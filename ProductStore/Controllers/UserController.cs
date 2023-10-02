﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using ProductStore.ConfigurationError;
using ProductStore.Core.Interface;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Framework.Configuration;
using ProductStore.Framework.Pagination;
using ProductStore.Framework.Services;
using ProductStore.Interface;
using ProductStore.Models;
using ProductStore.Repository;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
/*using System.Web.Mvc;*/

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IServicePagination<User> _servicePagination;
        private readonly DataContext _dataContext;
        private readonly ICreateJWT _createJWT;
        public UserController(IUserRepository userRepository, UserManager<User> userManager, IServicePagination<User> servicePagination, DataContext dataContext, ICreateJWT createJWT) 
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _servicePagination = servicePagination;
            _dataContext = dataContext;
            _createJWT = createJWT;
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
        public async Task<IActionResult> GetUserById(Guid id)
        {
            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }
            return Ok(await _userRepository.GetUserById(id));
        }

        [HttpGet("api/CustomerPagination")]
        public async Task<IActionResult> GetCustomerPagination([FromQuery] PaginationFilter filter)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            //IQueryable<Customer> query = _dataContext.Customers;
            var customers = await _servicePagination.GetCustomersPagination(_dataContext.Users, filter);

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

        [HttpGet("ExportExcel")]
        public IActionResult ExportExcel()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                DataTable dataTable = GetAddressData();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("User");

                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                    }

                    for (int row = 0; row < dataTable.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataTable.Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                        }
                    }

                    var tableRange = worksheet.Cells[1, 1, dataTable.Rows.Count + 1, dataTable.Columns.Count];
                    var table = worksheet.Tables.Add(tableRange, "UserTable");
                    table.TableStyle = TableStyles.Light1;

                    var fileBytes = package.GetAsByteArray();
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "User.xlsx");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in Excel {ex.Message}");
            }
        }

        private DataTable GetAddressData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "User";
            dt.Columns.Add("Id", typeof(Guid));
            dt.Columns.Add("ImageProfil", typeof(byte));
            /*dt.Columns.Add("VericationToken", typeof(string));
            dt.Columns.Add("VerifiedAt", typeof(DateTime));
            dt.Columns.Add("PasswordResetToken", typeof(string));
            dt.Columns.Add("ResetTokenExpires", typeof(DateTime));*/
            dt.Columns.Add("UserName", typeof (string));
/*            dt.Columns.Add("NormalizedUserName", typeof (string));
            dt.Columns.Add("Email", typeof(string));*/

            var categoryData = _userRepository.GetUsers().Result;
            foreach (var customer in categoryData)
            {
                dt.Rows.Add(customer.Id, customer.ImageProfile, customer.UserName);

            }

            return dt;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest();
            }

            var existingUser = await _userManager.FindByNameAsync(userDTO.UserName);
            if (existingUser != null)
            {
                throw new ExistModel("User");
            }

            var user = new User
            {
                Email = userDTO.Email,
                UserName = userDTO.UserName
            };

            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (result.Succeeded)
            {
                var verificationToken = _createJWT.CreateJwt(user);
                user.VerificationToken = verificationToken;
                await _userManager.UpdateAsync(user); 

                return Ok("Successfully created");
            }
            else
            {
                return BadRequest(result.Errors); 
            }
        }

        [HttpPost("AddImageProfile/{userId}")]
        public async Task<IActionResult> AddImageProfile(Guid userId, [FromForm] IFormFile image)
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
        public async Task<IActionResult> UpdateImageProfile(Guid userId, [FromForm] IFormFile image)
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
        public async Task<IActionResult> DeleteImageProfile(Guid userId)
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

        /*private string CreateJwt(User user)
        {
            *//*var tokenHandler = new JwtSecurityTokenHandler();

            var jwtKey = _configuration.GetSection("JwtSettings:Token").Value;

            var encodedJWTKey = Encoding.UTF8.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodedJWTKey), SecurityAlgorithms.HmacSha256Signature),
            };

            var roles = _userManager.GetRolesAsync(user).Result.ToList();

            roles.ForEach(role =>
            {
                tokenDescriptor.Subject.AddClaim(new Claim("roles", role));
            });

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var writtenToken = tokenHandler.WriteToken(token);

            return writtenToken;*//*
            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Token));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }*/


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRegister request)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

/*            if (!VerifiedPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password is incorrect");
            }*/

            if (user.VerifiedAt == null)
            {
                return BadRequest("Not veriffied!");
            }

            //user.PasswordResetToken = CreateJwt(user);
            user.PasswordResetToken = _createJWT.CreateJwt(user);

            return Ok(new { token = user.PasswordResetToken });

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

            user.PasswordResetToken = _createJWT.CreateJwt(user); 
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            _userRepository.Save();

            return Ok("You may now reset your password");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> RessetPassword(ResetPassword request)
        {
            /* var user = await _userRepository.GetUserByToken(request.Token);
             if (user == null || user.ResetTokenExpires < DateTime.Now)
             {
                 return BadRequest("User not found!");
             }

             CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
             user.PasswordResetToken = null;
             user.ResetTokenExpires = null;
             _userRepository.Save();*/
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
            {
                return BadRequest("User not found!");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

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


        [HttpDelete("deleteUser/{id}")]
        [Authorize(Roles = "admin")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            //var f = User.Identity;
            if (!_userRepository.ExistUser(id))
            {
                throw new AppException("User", id.ToString());
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
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDTOUpdate, string id)
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
