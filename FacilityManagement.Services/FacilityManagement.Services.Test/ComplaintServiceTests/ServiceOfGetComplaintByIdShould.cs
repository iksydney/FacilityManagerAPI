using FacilityManagement.Services.Core.Implementation;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using FacilityManagement.Services.Test.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test.FeedControllerTest
{
    public class GetComplaintByIdShould
    {
        private IServiceProvider _serviceProvider;
        public Mock<IComplaintRepository> MockComplaintsRepo { get; set; } = new Mock<IComplaintRepository>();
        public Mock<IFeedRepository> MockFeedRepo { get; set; } = new Mock<IFeedRepository>();
        public Mock<IUserRepository> MockUserRepo { get; set; } = new Mock<IUserRepository>();
        public Mock<IImageService> MockImageService { get; set; } = new Mock<IImageService>();

        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintRepository))).Returns(MockComplaintsRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IFeedRepository))).Returns(MockFeedRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IUserRepository))).Returns(MockUserRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IImageService))).Returns(MockImageService.Object).Verifiable();

            _serviceProvider = mockServiceProvider.Object;
        }
        [Test]
        public async Task Test_GetFeeds_Should_Return_Success_As_True_When_A_Valid_Object_Is_Gotten()
        {
            //Arrange
            MockUp(ModelReturnHelper.ReturnComplaint());
            var ComplaintServices = new ComplaintServices(_serviceProvider);
            var expectedState = true;
            var expectedMessage = "Complaint";

            //ACT
            var actual = await ComplaintServices.GetComplaintByComplaintId("Id");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Data);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
            Assert.IsInstanceOf<Response<ComplaintsDTO>>(actual);
        }

        [Test]
        public async Task Test_GetFeeds_Should_Return_Success_As_False_When_An_InValid_Object_Is_Gotten()
        {
            //Arrange
            MockUp(null);
            var ComplaintServices = new ComplaintServices(_serviceProvider);
            var expectedState = false;
            var expectedMessage = "Complaint Id not found";

            //ACT
            var actual = await ComplaintServices.GetComplaintByComplaintId("Id");

            //Assert
            Assert.IsNotNull(actual);
            Assert.Null(actual.Data);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        private void MockUp(Complaint Complaint)
        {
            MockComplaintsRepo.Setup(service => service.GetComplaintById(It.IsAny<string>())).Returns(Task.FromResult(Complaint));
        }
    }
}
