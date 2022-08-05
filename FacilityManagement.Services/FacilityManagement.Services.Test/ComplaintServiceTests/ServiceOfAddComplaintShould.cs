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
    class ServiceOfAddComplaintShould
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
            MockUp2(new Category(), new User());
            var complaintServices = new ComplaintServices(_serviceProvider);
            var feedId = "";
            var complaint = new AddComplaintDTO
            {
                UserId = ""
            };

            //Act
            var actual = await complaintServices.AddComplaint(feedId, complaint);

            //Assert
            Assert.IsNotNull(actual.Data);
        }

        [Test]
        public async Task FailWhenCategoryNotFound()
        {
            //Arrange
            MockUp(null);
            var complaintServices = new ComplaintServices(_serviceProvider);
            var feedId = "";
            var complaint = new AddComplaintDTO();

            //Act
            var actual = await complaintServices.AddComplaint(feedId, complaint);

            //Assert
            Assert.IsNull(actual.Data);
        }

        private void MockUp(Category feed)
        {
            MockComplaintsRepo.Setup(service => service.Add(It.IsAny<Complaint>()))
                .Returns(Task.FromResult(true));
            MockFeedRepo.Setup(service => service.GetById(It.IsAny<string>()))
                .Returns(Task.FromResult(feed));
            MockFeedRepo.Setup(services => services.Modify(It.IsAny<Category>()))
                .Returns(Task.FromResult(true));
        }

        private void MockUp2(Category feed, User user)
        {
            MockUserRepo.Setup(service => service.GetById(It.IsAny<string>()))
                .Returns(Task.FromResult(user));
            MockFeedRepo.Setup(service => service.GetById(It.IsAny<string>()))
                .Returns(Task.FromResult(feed));
        }
    }
}
