﻿using FacilityManagement.Services.Core.Implementation;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test
{
    class DeleteReplyShould
    {
        private IServiceProvider _serviceProvider;
        public Mock<IReplyRepository> MockRepliesRepo { get; set; } = new Mock<IReplyRepository>();
        public Mock<ICommentRepository> MockCommentRepo { get; set; } = new Mock<ICommentRepository>();
        public Mock<IConfiguration> MockConfig { get; set; } = new Mock<IConfiguration>();


        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IReplyRepository))).Returns(MockRepliesRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IConfiguration))).Returns(MockConfig.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(ICommentRepository))).Returns(MockCommentRepo.Object).Verifiable();

            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task TestGetFeedsValid()
        {
            //Arrange
            MockUp(new Replies(), true);
            var repliesServices = new ReplyService(_serviceProvider);
            var expectedState = true;
            var expectedMessage = "successfully deleted reply";

            //ACT
            var actual = await repliesServices.DeleteReply("Id");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        [Test]
        public async Task TestGetFeedsInValid()
        {
            //Arrange
            MockUp(null, false);
            var repliesServices = new ReplyService(_serviceProvider);
            var expectedState = false;
            var expectedMessage = "Invalid reply id provided";

            //ACT
            var actual = await repliesServices.DeleteReply("Id");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        [Test]
        public async Task TestGetFeedsEdgeCase()
        {
            //Arrange
            MockUp(new Replies(), false);
            var repliesServices = new ReplyService(_serviceProvider);
            var expectedState = false;
            var expectedMessage = "Something went wrong we're working on it";

            //ACT
            var actual = await repliesServices.DeleteReply("Id");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        private void MockUp(Replies reply, bool state)
        {
            MockRepliesRepo.Setup(service => service.GetById(It.IsAny<string>())).Returns(Task.FromResult(reply));
            MockRepliesRepo.Setup(service => service.DeleteById(It.IsAny<string>())).Returns(Task.FromResult(state));
        }
    }
}
