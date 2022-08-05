using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
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
    public class OtherTests
    {
        private IServiceProvider _serviceProvider;
        private FeedController _feedController;

        //mock IFeedServices
        private readonly Mock<IFeedService> MockFeedServices = new Mock<IFeedService>();
        public Mock<IComplaintService> MockComplaintService  = new Mock<IComplaintService>();
        public Mock<IReplyService> MockRepliesService  = new Mock<IReplyService>();
        public Mock<ICommentRepository> MockCommentsRepo = new Mock<ICommentRepository>();
        public Mock<IReplyRepository> MockReplyRepo = new Mock<IReplyRepository>();
        public Mock<IRatingService> MockRatingsService = new Mock<IRatingService>();
        public Mock<ICommentService> MockCommentService { get; set; } = new Mock<ICommentService>();


        [SetUp]
        public void SetUp()
        {
            ////set up mock ICommentsRepo
            MockCommentsRepo.Setup(method => method.GetById(It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<Comments>()));
            //set up mock IReplyRepository
            MockReplyRepo.Setup(callMethod => callMethod.GetById(It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<Replies>()));
            //set Up edit comment
            MockCommentService.Setup(callMethod => callMethod.GetCommentById(It.IsAny<string>())).Returns(Task.FromResult(new Response<Comments>()));


            //mock UserManager
            var store = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            

            //mock IService Provider
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);

            mockServiceProvider.Setup(injector => injector.GetService(typeof(IFeedService))).Returns(MockFeedServices.Object).Verifiable();
            mockServiceProvider.Setup(injector => injector.GetService(typeof(UserManager<User>))).Returns(userManager.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(ICommentService))).Returns(MockCommentService.Object).Verifiable();
            userManager.Setup(manager => manager.GetUserAsync(new ClaimsPrincipal())).Returns(Task.FromResult(new User()));
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IReplyService))).Returns(MockRepliesService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintService))).Returns(MockComplaintService.Object).Verifiable();
            mockServiceProvider.Setup(inject => inject.GetService(typeof(IRatingService))).Returns(MockRatingsService.Object);


            //assign data
            _serviceProvider = mockServiceProvider.Object;
            _feedController = new FeedController(_serviceProvider);

        }

        [Test]
        public async Task EditCommentTest()
        {
            //arrange
            string idFromRoute = "081450e9-40d3-484f-b4e9-4b8136f45ebd";

            var commentDto = new EditCommentDTO { Comment = "hey" };

            //act
            EditCommentByIdMockUp(true);
            var editCommentApiResult = await _feedController.EditComment(idFromRoute, commentDto) as NoContentResult;

            EditCommentByIdMockUp(false);
            var editCommentApiResult1 = await _feedController.EditComment("", commentDto) as BadRequestObjectResult;

            //assert
            Assert.NotNull(editCommentApiResult);
            Assert.IsInstanceOf<NoContentResult>(editCommentApiResult);
            Assert.IsInstanceOf<BadRequestObjectResult>(editCommentApiResult1);

        }

        [Test]
        public async Task AddReplyTest()
        {
            //arrange
            var addReplyDto = new AddReplyDTO() { Reply = "hi guys, its Easter already" };

            //act
            AddReplyMockUp(true);
            var editReplyApi = await _feedController.AddReply("commentId", addReplyDto) as CreatedResult;

            AddReplyMockUp(false);
            var editReplyApi1 = await _feedController.AddReply("commentId", addReplyDto) as BadRequestObjectResult;

            //assert
            Assert.IsInstanceOf<CreatedResult>(editReplyApi);
            Assert.IsInstanceOf<BadRequestObjectResult>(editReplyApi1);
        }

        [Test]
        public async Task EditReplyTest()
        {
            //arrange
            string idFromRoute = "";
            EditReplyDTO editReplyDto = new EditReplyDTO { Reply = "" };

            //act
            EditReplyMockUp(true);
            var editReplyApiResult = await _feedController.EditReply(idFromRoute, editReplyDto) as NoContentResult;

            EditReplyMockUp(false);
            var editReplyApiResult1 = await _feedController.EditReply(idFromRoute, editReplyDto) as BadRequestObjectResult;

            //assert
            Assert.NotNull(editReplyDto);
            Assert.IsInstanceOf<NoContentResult>(editReplyApiResult);
            Assert.IsInstanceOf<BadRequestObjectResult>(editReplyApiResult1);
        }

        //set up for Edit comment
        public void EditCommentByIdMockUp(bool val)
        {
            //bool val = !string.IsNullOrWhiteSpace(res);
            MockCommentService.Setup(callMethod =>
                    callMethod.EditCommentById(It.IsAny<EditCommentDTO>(), It.IsAny<string>(), It.IsAny<User>()))
                .Returns(Task.FromResult(new Response<Comments> { Success = val }));
        }

        //set up for Add  Reply
        public void AddReplyMockUp(bool val)
        {
            MockRepliesService.Setup(callMethod =>
                    callMethod.CreateReply(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<AddReplyDTO>()))
                .Returns(Task.FromResult(new Response<ReplyDTO> { Success = val }));
        }


        ///set up for Edit Reply
        public void EditReplyMockUp(bool val)
        {
            MockRepliesService.Setup(callMethod =>
                    callMethod.EditReply(It.IsAny<EditReplyDTO>(), It.IsAny<string>(), It.IsAny<User>()))
                .Returns(Task.FromResult(new Response<string> { Success = val }));
        }

    }
}
