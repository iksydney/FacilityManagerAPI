﻿using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test
{
    class FeedControllerTestDeleteComment
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
        public async Task TestDeleteCommentValid()
        {
            //Arrange
            MockUp(true);
            var feedController = new FeedController(_serviceProvider);
            var expected = 200;

            //ACT
            var actual = await feedController.DeleteComment("commentId") as OkObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        [Test]
        public async Task TestDeleteCommentInValid()
        {
            //Arrange
            MockUp(false);
            var feedController = new FeedController(_serviceProvider);
            var expected = 400;

            //ACT
            var actual = await feedController.DeleteComment("commentId") as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        private void MockUp(bool state)
        {
            MockCommentService.Setup(service => service.DeleteComment(It.IsAny<string>())).Returns(Task.FromResult(new Response<string> { Success = state }));
        }

    }
}
