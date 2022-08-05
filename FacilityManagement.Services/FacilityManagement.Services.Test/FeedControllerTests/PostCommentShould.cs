using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace FacilityManagement.Services.Test
{
    public class PostCommentShould
    {
        private IServiceProvider _serviceProvider;
        public Mock<IComplaintService> MockComplaintService { get; set; } = new Mock<IComplaintService>();
        public Mock<IFeedService> MockFeedServices { get; set; } = new Mock<IFeedService>();
        public Mock<ICommentService> MockCommentService { get; set; } = new Mock<ICommentService>();
        public Mock<IReplyService> MockRepliesService { get; set; } = new Mock<IReplyService>();
        public Mock<IRatingService> MockRatingsService { get; set; } = new Mock<IRatingService>();
        private readonly static Mock<IUserStore<User>> Store = new Mock<IUserStore<User>>();
        private readonly Mock<UserManager<User>> MockUserManager = new Mock<UserManager<User>>(Store.Object, null, null, null, null, null, null, null, null);

        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IFeedService))).Returns(MockFeedServices.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintService))).Returns(MockComplaintService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(ICommentService))).Returns(MockCommentService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IReplyService))).Returns(MockRepliesService.Object).Verifiable();
            mockServiceProvider.Setup(service => service.GetService(typeof(IRatingService)))
                .Returns(MockRatingsService.Object);
            mockServiceProvider.Setup(service => service.GetService(typeof(UserManager<User>)))
                .Returns(MockUserManager.Object);
            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task TestPostCommentsIsNotValid()
        {
            //Arrange
            MockUp(false);
            var feedController = new FeedController(_serviceProvider);

            //ACT
            var actual = await feedController.Comment("", new CommentDto(){Comment = "Yes, the lights are bad" }) as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        [Test]
        public async Task TestPostCommentsIsValid()
        {
            //Arrange
            MockUp(true);
            var feedController = new FeedController(_serviceProvider);

            //ACT
            var actual = await feedController.Comment("", new CommentDto() { Comment = "Yes, the lights are bas"}) as ObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(StatusCodes.Status201Created, actual.StatusCode);
        }

        private void MockUp(bool state)
        {
            MockCommentService.Setup(service => service.PostComment("", It.IsAny<User>(), It.IsAny<CommentDto>())).
               Returns(Task.FromResult(new Response<string>{Success = state}));
        }
    }
}