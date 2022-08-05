﻿using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test
{
    public class FeedControlllerTestRateComplain
    {
        private IServiceProvider _serviceProvider;
        public Mock<IReplyService> MockRepliesService { get; set; } = new Mock<IReplyService>();
        public Mock<IComplaintService> MockComplaintService { get; set; } = new Mock<IComplaintService>();
        public Mock<IFeedService> MockFeedServices { get; set; } = new Mock<IFeedService>();
        public Mock<IRatingService> MockRatingService { get; set; } = new Mock<IRatingService>();
        public Mock<ICommentService> MockCommentService { get; set; } = new Mock<ICommentService>();
        public static Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        public Mock<UserManager<User>> userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);


        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IFeedService))).Returns(MockFeedServices.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintService))).Returns(MockComplaintService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(ICommentService))).Returns(MockCommentService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IRatingService))).Returns(MockRatingService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IReplyService))).Returns(MockRepliesService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(UserManager<User>))).Returns(userManager.Object)
                .Verifiable();
            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task TestRateComplainValid()
        {
            //Arrange
            MockUp(true);
            var feedController = new FeedController(_serviceProvider);
            var expected = 201;
            var ratingDTO = new RatingDTO();

            //ACT
            var actual = await feedController.RateComplaint("", ratingDTO) as CreatedResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        [Test]
        public async Task TestRateComplainInValid()
        {
            //Arrange
            MockUp(false);
            var feedController = new FeedController(_serviceProvider);
            var expected = 400;
            var ratingDTO = new RatingDTO();

            //ACT
            var actual = await feedController.RateComplaint("", ratingDTO) as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        private void MockUp(bool state)
        {
            userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).Returns(Task.FromResult(new User {Id ="" }));
            MockRatingService.Setup(service => service.RateComplain(It.IsAny<string>(),It.IsAny<string>(), It.IsAny<RatingDTO>())).Returns(Task.FromResult(new Response<RatingToReturnDTO> { Success = state, Data = new RatingToReturnDTO()}));
        }
    }
}