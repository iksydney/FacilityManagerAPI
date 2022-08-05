using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FacilityManagement.Services.Test
{
    public class ServiceOfEditComplaintShould
    {
        private IServiceProvider _serviceProvider;
        public Mock<IComplaintService> MockComplaintService { get; set; } = new Mock<IComplaintService>();
        public Mock<IFeedService> MockFeedService { get; set; } = new Mock<IFeedService>();
        public Mock<IReplyService> MockRepliesService { get; set; } = new Mock<IReplyService>();
        public Mock<IRatingService> MockRatingService { get; set; } = new Mock<IRatingService>();
        public Mock<ICommentService> MockCommentService { get; set; } = new Mock<ICommentService>();

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IFeedService))).Returns(MockFeedService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintService))).Returns(MockComplaintService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(ICommentService))).Returns(MockCommentService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IRatingService))).Returns(MockRatingService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IReplyService))).Returns(MockRepliesService.Object).Verifiable();
            mockServiceProvider.Setup(injector => injector.GetService(typeof(UserManager<User>)))
                .Returns(userManager.Object).Verifiable();
            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task OkResponse()
        {
            //Arrange
            MockUp(true);
            var feedController = new FeedController(_serviceProvider);
            var complaintid = "";
            var complaint = new EditComplaintDTO();
            var expected = 200;

            //ACT
            var actual = await feedController.EditComplaint(complaintid, complaint) as OkObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        [Test]
        public async Task BadRequestResponse()
        {
            //Arrange
            MockUp(false);
            var feedController = new FeedController(_serviceProvider);
            var complaintid = "";
            var complaint = new EditComplaintDTO();
            var expected = 400;

            //ACT
            var actual = await feedController.EditComplaint(complaintid, complaint) as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        private void MockUp(bool status)
        {
            MockComplaintService.Setup(service => service.EditComplaint(It.IsAny<string>(), It.IsAny<EditComplaintDTO>()))
               .Returns(Task.FromResult(new Response<string> { Success = status }));
        }
    }
}