using FacilityManagement.Services.Core.Implementation;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test.ComplaintServiceTests
{
    public class ServiceOfEditComplaintShould
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
        public async Task ReturnValidResponse()
        {
            //Arrange
            MockUp2(new Complaint(), new User());
            var complaintServices = new ComplaintServices(_serviceProvider);
            var complaintId = "";
            var complaint = new EditComplaintDTO
            {
                UserId = ""
            };

            //Act
            var actual = await complaintServices.EditComplaint(complaintId, complaint);

            //Assert
            Assert.IsTrue(actual.Success);
        }

        [Test]
        public async Task FailWhenComplaintNotFound()
        {
            //Arrange
            MockUp(null);
            var complaintServices = new ComplaintServices(_serviceProvider);
            var complaintId = "";
            var complaint = new EditComplaintDTO();

            //Act
            var actual = await complaintServices.EditComplaint(complaintId, complaint);

            //Assert
            Assert.IsFalse(actual.Success);
        }

        private void MockUp(Complaint complaint)
        {
            MockComplaintsRepo.Setup(service => service.GetById(It.IsAny<string>()))
                .Returns(Task.FromResult(complaint));
            MockComplaintsRepo.Setup(service => service.Modify(complaint))
                .Returns(Task.FromResult((complaint != null)));
        }

        private void MockUp2(Complaint complaint, User user)
        {
            MockUserRepo.Setup(service => service.GetById(It.IsAny<string>()))
                .Returns(Task.FromResult(user));
            MockComplaintsRepo.Setup(service => service.GetById(It.IsAny<string>()))
                .Returns(Task.FromResult(complaint));
            MockComplaintsRepo.Setup(service => service.Modify(complaint))
                .Returns(Task.FromResult((complaint != null)));
        }
    }
}
