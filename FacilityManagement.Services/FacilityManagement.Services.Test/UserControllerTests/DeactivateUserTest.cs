﻿using FacilityManagement.Services.API.Controllers;
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
    public class DeactivateUserTest
    {
        public IServiceProvider _serviceProvider;
        public static Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        public Mock<UserManager<User>> MockUserManager { get; set; } = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        public Mock<IUserService> MockUserService { get; set; } = new Mock<IUserService>();

        [SetUp]
        public void SetUp()
        {

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(UserManager<User>))).
                Returns(MockUserManager.Object).Verifiable();
            mockServiceProvider.Setup(x => x.GetService(typeof(IUserService))).
               Returns(MockUserService.Object).Verifiable();

            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task TestDeactivateUserValid()
        {
            //Arrange
            var userController = new UserController(_serviceProvider);
            MockReturn(true);
            var expected = 204;

            //Act
            var actual = await userController.DeactivateUser("") as NoContentResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        [Test]
        public async Task TestDeactivateUserInvalid()
        {
            //Arrange
            var userController = new UserController(_serviceProvider);
            MockReturn(false);
            var expected = 400;

            //Act
            var actual = await userController.DeactivateUser("") as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        public void MockReturn(bool state)
        {
            MockUserService.Setup(x => x.DeactivateUser(It.IsAny<string>())).Returns(Task.FromResult(new Response<string> { Success = state }));
        }
    }
}
