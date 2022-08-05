﻿using FacilityManagement.Services.Core.Implementation;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test
{
    public class ComplaintServiceTestDeleteComplaintById
    {
        private IServiceProvider _serviceProvider;
        public Mock<IComplaintRepository> MockComplaintRepo { get; set; } = new Mock<IComplaintRepository>();
        public Mock<IUserRepository> MockUserRepo { get; set; } = new Mock<IUserRepository>();
        public Mock<IFeedRepository> MockFeedRepo { get; set; } = new Mock<IFeedRepository>();
        public Mock<IImageService> MockImageService { get; set; } = new Mock<IImageService>();


        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintRepository))).Returns(MockComplaintRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IFeedRepository))).Returns(MockFeedRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IUserRepository))).Returns(MockUserRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IImageService))).Returns(MockImageService.Object).Verifiable();
            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task TestDeleteComplaintByIdValid()
        {
            //Arrange
            MockUp(new Complaint(), true);
            var complaintServices = new ComplaintServices(_serviceProvider);
            var expectedState = true;
            var expectedMessage = "Complaint deleted successfully";

            //ACT
            var actual = await complaintServices.DeleteComplaint("Id");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        [Test]
        public async Task TestDeleteComplaintByIdInValid()
        {
            //Arrange
            MockUp(null, false);
            var complaintServices = new ComplaintServices(_serviceProvider);
            var expectedState = false;
            var expectedMessage = "Invalid complaint Id";

            //ACT
            var actual = await complaintServices.DeleteComplaint("Id");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        [Test]
        public async Task TestDeleteComplaintByIdEdgeCase()
        {
            //Arrange
            MockUp(new Complaint(), false);
            var complaintServices = new ComplaintServices(_serviceProvider);
            var expectedState = false;
            var expectedMessage = "Unable to delete complaint, Plese try again later";

            //ACT
            var actual = await complaintServices.DeleteComplaint("Id");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        private void MockUp(Complaint complaint, bool state)
        {
            MockComplaintRepo.Setup(service => service.GetComplaintById(It.IsAny<string>())).Returns(Task.FromResult(complaint));
            MockComplaintRepo.Setup(service => service.DeleteById(It.IsAny<string>())).Returns(Task.FromResult(state));
        }
    }

   
}
