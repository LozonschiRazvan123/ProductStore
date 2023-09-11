using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Controllers;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreTest.Controller
{
    public class AddressControllerTest
    {
        private AddressController _addressController;
        private IAddressRepository _addressRepository;
        public AddressControllerTest()
        {
            //Dependencies
            _addressRepository = A.Fake<IAddressRepository>();

            //SUT
            //System under test (SUT) refers to a system that is being tested for correct operation.
            //According to ISTQB it is the test object. From a unit testing perspective, the system under test represents
            //all of the classes in a test that are not predefined pieces of code like stubs or even mocks.
            _addressController = new AddressController(_addressRepository);
        }


        [Fact]
        public void ClubController_GetAddress_ReturnsSuccess()
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
    }
}
