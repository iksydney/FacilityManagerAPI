using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test
{
    public class GetUserTest
    {
        public IServiceProvider _serviceProvider;
        public static Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        public Mock<UserManager<User>> MockUserManager { get; set; } = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        public Mock<IUserService> MockUserService { get; set; } = new Mock<IUserService>();

        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(UserManager<User>))).
                Returns(MockUserManager.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IUserService))).
               Returns(MockUserService.Object).Verifiable();

            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task TestGetUserValid()
        {
            //Arrange
            var userController = new UserController(_serviceProvider);
            MockReturn(true);
            var expected = 200;

            //Act
            var actual = await userController.GetUser("") as OkObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        [Test]
        public async Task TestGetUsersInvalid()
        {
            //Arrange
            var userController = new UserController(_serviceProvider);
            MockReturn(false);
            var expected = 400;

            //Act
            var actual = await userController.GetUser("") as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        public void MockReturn(bool state)
        {
            MockUserService.Setup(x => x.GetUser(It.IsAny<string>())).Returns(Task.FromResult(new Response<UserDTO>{Success = state}));
        }
    }
}
