using Microsoft.AspNetCore.Authorization;
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
        private readonly IGetDataExcel _excel;
        private readonly IImportDataExcel _importDataExcel;
        public UserController(IUserRepository userRepository, UserManager<User> userManager, IServicePagination<User> servicePagination, DataContext dataContext, ICreateJWT createJWT, IGetDataExcel excel, IImportDataExcel importDataExcel) 
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _servicePagination = servicePagination;
            _dataContext = dataContext;
            _createJWT = createJWT;
            _excel = excel;
            _importDataExcel = importDataExcel;
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
                DataTable dataTable = _excel.GetUserData();

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

        [HttpPost("ImportExcel")]
        public IActionResult ImportExcel(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        _importDataExcel.ImportDataFromExcelUser(file);
                    }

                    return Ok("Awsome");
                }
                else
                {
                    return BadRequest("Naspa");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
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

        [HttpPut("ImportExcel")]
        public IActionResult ImportExcelUser(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        _importDataExcel.ImportDataExcelUpdateUser(file);
                    }

                    return Ok("Awsome");
                }
                else
                {
                    return BadRequest("Naspa");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

    }
}
