using Castle.Core.Configuration;
using FakeItEasy;
using FakeItEasy.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProductStore.Controllers;
using ProductStore.DTO;
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
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public UserControllerTest()
        {
            _userRepository = A.Fake<IUserRepository>();
            _userManager = A.Fake<UserManager<User>>();


            var configuration = Configure();
            var services = new ServiceCollection();

            
            services.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configuration);

            //_userController = A.Fake<UserController>();

            _userController = A.Fake<UserController>(builder => builder.WithArgumentsForConstructor(new object[] { _userRepository, configuration, _userManager }));
        }

        public static Microsoft.Extensions.Configuration.IConfiguration Configure()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                 .AddEnvironmentVariables()
                 .Build();

            return config;
        }

        [Fact]
        public void UserController_GetUsers()
        {
            var users = new List<UserDTO>();
            A.CallTo(() => _userRepository.GetUsers()).Returns(users);
            var result = _userController.GetUsers();
            result.Should().BeOfType<Task<IActionResult>>();
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
