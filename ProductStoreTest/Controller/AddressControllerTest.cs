using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductStore.Controllers;
using ProductStore.Core.Interface;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Framework.Services;
using ProductStore.Interface;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreTest.Controller
{
    public class AddressControllerTest
    {
        private AddressController _addressController;
        private IAddressRepository _addressRepository;
        private readonly IServicePagination<Address> _servicePagination;
        private readonly DataContext _dataContext;
        private readonly GetDataExcel _excel;
        public AddressControllerTest()
        {
            //Dependencies
            _addressRepository = A.Fake<IAddressRepository>();
            _servicePagination = A.Fake<IServicePagination<Address>>();
            _dataContext = new DataContext(new DbContextOptions<DataContext>());
            _excel = A.Fake<GetDataExcel>();

            //SUT
            //System under test (SUT) refers to a system that is being tested for correct operation.
            //According to ISTQB it is the test object. From a unit testing perspective, the system under test represents
            //all of the classes in a test that are not predefined pieces of code like stubs or even mocks.
            _addressController = new AddressController(_addressRepository, _servicePagination, _dataContext, _excel);
        }


        [Fact]
        public void AddressController_GetAddress_ReturnsSuccess()
        {
            // Arrange - What do I need to bring in?
            var addresses = new List<AddressDTO>();
            var fakeAddressRepository = A.Fake<IAddressRepository>();
            A.CallTo(() => fakeAddressRepository.GetAddresses()).Returns(addresses);

            //Act  
            var result = _addressController.GetAddress();

            //Assert - Object check actions
            result.Should().BeOfType<OkObjectResult>();
        }


        [Fact]
        public void AddressController_GetAddressID_ReturnsSuccess()
        {
            var id = 1;
            var address = A.Fake<AddressDTO>();
            A.CallTo(() => _addressRepository.GetAddress(id)).Returns(address);
            var result = _addressController.GetAddressById(id);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void AddressController_CreateAddress_ReturnsCreateResult()
        {
            var addressDTO = new AddressDTO();
            /*A.CallTo(() => _addressRepository.CreateAddress(addressDTO)).Returns(true);

            var controller = new AddressController(_addressRepository);

            // Act
            var result = controller.CreateAddress(addressDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);*/
            A.CallTo(() => _addressRepository.CreateAddress(addressDTO)).Returns(true);
            var result = _addressController.CreateAddress(addressDTO);
            result.Should().BeOfType<OkObjectResult>();
            //Assert.Equal("Successfully created!", okResult.Value);
        }

        [Fact]
        public void AddressController_CreateAddress_ReturnsBadRequestWhenDTOIsNull()
        {
            AddressDTO addressDTO = null;
            A.CallTo(() => _addressRepository.CreateAddress(addressDTO)).Returns(true);
            var result = _addressController.CreateAddress(addressDTO);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void AddressController_UpdateAddress_SuccessfullyUpdate()
        {
            var addressId = 1;
            var updateAddress = new AddressDTO
            {
                Id = addressId,
                Street = "Update",
                City = "Update",
                State = "Update"
            };

            A.CallTo(() => _addressRepository.UpdateAddress(updateAddress)).Returns(true);
            var result = _addressController.UpdateAddress(addressId, updateAddress);
            result.Should().BeOfType<OkObjectResult>();

        }

        [Fact]
        public void AddressController_UpdateAddress_ReturnsBadRequestWhenDTOIsInvalid()
        {
            var addressId = 10;
            AddressDTO updateAddress = null;

            A.CallTo(() => _addressRepository.UpdateAddress(updateAddress)).Returns(true);
            var result = _addressController.UpdateAddress(addressId, updateAddress);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void AddressController_DeleteAddress_SuccessfullyUpdate()
        {
            var addressId = 1;
            var address = A.Fake<AddressDTO>();
            A.CallTo(() => _addressRepository.AddressExist(addressId)).Returns(true);
            A.CallTo(() => _addressRepository.GetAddress(addressId)).Returns(address);
            A.CallTo(() => _addressRepository.DeleteAddress(address)).Returns(true);
            var result = _addressController.DeleteAddress(addressId);
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
