using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test.TestsForUserController
{
    public class GetUsersBySquadEndpoint
    {
        private UserController _userController;
        private IServiceProvider _serviceProvider;
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly static Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        private readonly Mock<UserManager<User>> _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        private readonly Response<Pagination<UserDTO>> _pagedUsers = Helper.GetPaginatedUsers;
        private readonly Response<Pagination<UserDTO>> _invalidPagedUsers = Helper.GetPaginatedUsersInvalid;

        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IUserService))).Returns(_userServiceMock.Object).Verifiable();
            mockServiceProvider.Setup(x => x.GetService(typeof(UserManager<User>))).Returns(_userManagerMock.Object).Verifiable();
            _serviceProvider = mockServiceProvider.Object;

            _userController = new UserController(_serviceProvider);
        }
        [Test]
        public async Task GetUsersBySquadShouldReturnOkWhenSuccessful()
        {
            //Arrange
            var squad = "sq-001";
            var page = 1;
            _userServiceMock.Setup(i => i.GetUsersBySquad(page, squad)).ReturnsAsync(_pagedUsers);

            //Act
            var result = await _userController.GetUsersInSquad(page, squad) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.True(result is OkObjectResult);
            Assert.IsInstanceOf<Response<Pagination<UserDTO>>>(result.Value);
            _userServiceMock.Verify(i => i.GetUsersBySquad(page, squad), Times.Once);
        }

        [Test]
        public async Task GetUsersBySquadShouldReturnBadRequestWhenInvalid()
        {
            //Arrange
            var squad = "sq-002";
            var page = 1;
            _userServiceMock.Setup(i => i.GetUsersBySquad(page, squad)).ReturnsAsync(_invalidPagedUsers);

            //Act
            var result = await _userController.GetUsersInSquad(page, squad) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result.Value);
            Assert.True(result is BadRequestObjectResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            _userServiceMock.Verify(i => i.GetUsersBySquad(page, squad), Times.Once);
        }
    }
}
