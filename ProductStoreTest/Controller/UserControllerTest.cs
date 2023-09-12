using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Controllers;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;
using ProductStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreTest.Controller
{
    public class UserControllerTest
    {
        private UserController _userController;
        private IUserRepository _userRepository;

        public UserControllerTest()
        {
            _userRepository = A.Fake<IUserRepository>();
            _userController = A.Fake<UserController>();
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


    }
}
