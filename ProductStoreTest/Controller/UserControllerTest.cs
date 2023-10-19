using Castle.Core.Configuration;
using FakeItEasy;
using FakeItEasy.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductStore.Controllers;
using ProductStore.Core.Interface;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Framework.Configuration;
using ProductStore.Framework.Services;
using ProductStore.Interface;
using ProductStore.Models;
using ProductStore.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreTest.Controller
{
    public class UserControllerTest
    {
        private UserController _userController;
        private IUserRepository _userRepository;
        private UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ICreateJWT _createJWT;
        private readonly IServicePagination<User> _servicePagination;
        private readonly DataContext _dataContext;
        private readonly GetDataExcel _excel;
        private readonly ImportDataExcel _importExcel;
        private readonly ICategoryProductRepository _categoryProductRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAddressRepository _addressRepository;
        public UserControllerTest()
        {
            _userRepository = A.Fake<IUserRepository>();
            _userManager = A.Fake<UserManager<User>>();
            _jwtSettings = A.Fake<JwtSettings>();
            _createJWT = A.Fake<ICreateJWT>();
            _servicePagination = A.Fake<IServicePagination<User>>();
            _excel = A.Fake<GetDataExcel>();
            _dataContext = new DataContext(new DbContextOptions<DataContext>());
            _importExcel = _importExcel = new ImportDataExcel(
        _addressRepository, _dataContext, _categoryProductRepository,
        _customerRepository, _orderRepository, _productRepository, _userRepository);

            //var configuration = Configure();
            var services = new ServiceCollection();


            //services.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configuration);
            //services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            //_userController = A.Fake<UserController>();

            var jwtSettings = new JwtSettings
            {
                Token = "my top secret key lozonschi-constantin-razvan123 dadaadsdasdbmfdlgkvn",
            };

            var jwtSettingsOptions = Options.Create(jwtSettings); // Creați obiectul IOptions<JwtSettings> folosind Options.Create

            _createJWT = new CreateJWT(_userManager, jwtSettingsOptions);
            _userController = new UserController(_userRepository, _userManager,_servicePagination, _dataContext, _createJWT, _excel, _importExcel);
        }

        /*public static Microsoft.Extensions.Configuration.IConfiguration Configure()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                 .AddEnvironmentVariables()
                 .Build();

            return config;
        }*/

        [Fact]
        public async Task UserController_GetUsers()
        {
            var users = new List<UserDTO>();
            A.CallTo(() => _userRepository.GetUsers()).Returns(users);
            var result = await _userController.GetUsers();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void UserController_CreateUser_SuccessfullyCreated()
        {
            var user = new UserDTO();
            A.CallTo(() => _userRepository.Add(user)).Returns(true);
            var result = _userController.CreateUser(user);
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public async void UserController_Login_ReturnsOkResult()
        {
            var request = new UserLoginRegister
            {
                Email = "razvanlozonschi123@gmail.com",
                Password = "Coding@1234?"
            };

            var user = new User()
            {
                Email = request.Email,
                VerifiedAt = DateTime.UtcNow,
                UserName = "RazvanLozonschi"
            };

            A.CallTo(() => _userRepository.GetUserByEmail(user.Email)).Returns(user);
            IList<string> userRoles = new List<string> {"admin"}; 
            A.CallTo(() => _userManager.GetRolesAsync(user)).Returns(Task.FromResult(userRoles));
            var resultTask = _userController.Login(request);
            var result = await resultTask;

            result.Should().BeAssignableTo<OkObjectResult>();

            result.As<OkObjectResult>().StatusCode.Should().Be(200);
        }


        [Fact]
        public async void UserController_ForgotPassword_ReturnsSuccessfully()
        {
            var user = new User()
            {
                Email = "razvanlozonschi123@gmail.com",
                UserName = "RazvanLozonschi"
            };
            IList<string> userRoles = new List<string> { "admin" };
            A.CallTo(() => _userManager.GetRolesAsync(user)).Returns(Task.FromResult(userRoles));
            A.CallTo(() => _userRepository.GetUserByEmail(user.Email)).Returns(user);
            var resultTask = _userController.ForgotPassword(user.Email);
            var result = await resultTask;
            result.Should().BeAssignableTo<OkObjectResult>();

            result.As<OkObjectResult>().StatusCode.Should().Be(200);

        }

        [Fact]
        public async void UserController_RessetPassword_ReturnsSuccess()
        {
            var request = new ResetPassword
            {
                Email = "razvanlozonschi123@gmail.com",
                Password = "NewPassword123",
                ConfirmPassword = "NewPassword123"
            };

            var user = new User()
            {
                Email = "razvanlozonschi123@gmail.com",
                UserName = "RazvanLozonschi"
            };

            A.CallTo(() => _userRepository.GetUserByEmail(user.Email)).Returns(user);
            var result = await _userController.RessetPassword(request);
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().StatusCode.Should().Be(200);
        }


        [Fact]
        public async void UserController_Verify_ReturnsSuccess()
        {
            var token = "token";
            var user = new User()
            {
                Email = "razvanlozonschi123@gmail.com",
                UserName = "RazvanLozonschi"
            };

            A.CallTo(() => _userRepository.GetUserByToken(token)).Returns(user);
            var result = await _userController.Verify(token);
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().StatusCode.Should().Be(200);
        }

    }
}
