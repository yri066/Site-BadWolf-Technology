using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Controllers;
using BadWolfTechnology.Data;
using BadWolfTechnology.Data.Interfaces;
using BadWolfTechnology.Models;
using BadWolfTechnology.UnitTest.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit.Abstractions;

namespace BadWolfTechnology.UnitTest
{
    public class NewsControllerTest : IClassFixture<TestDatabaseFixture>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public TestDatabaseFixture Fixture { get; }

        public NewsControllerTest(TestDatabaseFixture fixture, ITestOutputHelper testOutputHelper)
        {
            Fixture = fixture;
        }

        [Fact]
        public async void CreateNews_ModelStateIsInvalid_ReturnsView()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var todayDate = new DateTime(2024, 04, 24, 10, 9, 21, 456);
            var news = new NewsEdit()
            {
                Title = "Заголовок",
                Text = "",
            };

            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeAuthService = new Mock<IAuthorizationService>();
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();
            var controller = new NewsController(context, fakeUserManager.Object, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);
            controller.ModelState.AddModelError("Text", "Поле Текст обязательно.");

            //Act
            var result = await controller.Create(news, null);

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void CreateNews_ModelStateIsValid_ReturnsRedirectToAction()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var todayDate = new DateTime(2024, 04, 24, 10, 9, 21, 456);
            var actionName = "Details";
            var news = new NewsEdit()
            {
                Title = "Заголовок",
                Text = "Добавлено описание тестовой новости.",
            };

            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeAuthService = new Mock<IAuthorizationService>();
            var fakeDateTime = new Mock<IDateTime>();
                fakeDateTime.Setup(fakeDateTime => fakeDateTime.UtcNow).Returns(todayDate);
            var fakeFileManager = new Mock<IFileManager>();
            var controller = new NewsController(context, fakeUserManager.Object, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);

            //Act
            var result = await controller.Create(news, null);

            //Assert
            var resultRedirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(actionName, resultRedirect.ActionName);
            var newsResult = context.News.Single(b => b.Text == news.Text);
            Assert.Equal(todayDate, newsResult.Created);
        }

        [Fact]
        public async void EditNews_NewsNotFound_ReturnsNotFound()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var newsId = new Guid("7CE19DA6-088A-4571-6924-08DC71341B5D");
            var todayDate = new DateTime(2024, 04, 24, 10, 9, 21, 456);
            var news = new NewsEdit()
            {
                Title = "Заголовок",
                Text = "Изменение текста новости.",
            };

            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeAuthService = new Mock<IAuthorizationService>();
            var fakeDateTime = new Mock<IDateTime>();
                fakeDateTime.Setup(fakeDateTime => fakeDateTime.UtcNow).Returns(todayDate);
            var fakeFileManager = new Mock<IFileManager>();
            var controller = new NewsController(context, fakeUserManager.Object, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);

            //Act
            var result = await controller.EditAsync(newsId, news, null);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void EditNews_ModelStateIsValid_ReturnsRedirectToAction()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var newsId = new Guid("CA090C8C-7ED4-4726-429D-08DC5C4FA310");
            var todayDate = new DateTime(2024, 04, 24, 10, 9, 21, 456);
            var actionName = "Details";
            var news = new NewsEdit()
            {
                Title = "Заголовок",
                Text = "Изменение текста новости.",
            };

            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeAuthService = new Mock<IAuthorizationService>();
            var fakeDateTime = new Mock<IDateTime>();
                fakeDateTime.Setup(fakeDateTime => fakeDateTime.UtcNow).Returns(todayDate);
            var fakeFileManager = new Mock<IFileManager>();
            var controller = new NewsController(context, fakeUserManager.Object, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);

            //Act
            var result = await controller.EditAsync(newsId, news, null);

            //Assert
            var resultRedirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(actionName, resultRedirect.ActionName);
            var newsResult = context.News.Single(b => b.Id == newsId);
            Assert.Equal(news.Title, newsResult.Title);
        }

        [Fact]
        public async void EditNews_NewsIdGuidDefault_ReturnsBadRequest()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var newsId = Guid.Empty;
            var todayDate = new DateTime(2024, 04, 24, 10, 9, 21, 456);
            var news = new NewsEdit()
            {
                Title = "Заголовок",
                Text = "Изменение текста новости.",
            };

            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeAuthService = new Mock<IAuthorizationService>();
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();
            var controller = new NewsController(context, fakeUserManager.Object, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);

            //Act
            var result = await controller.EditAsync(newsId, news, null);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void CreateComment_NewsNotFound_ReturnsNotFound()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var newsId = Guid.Empty;
            const string commentText = "Добавлен новый тестовый комментарий";

            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeAuthService = new Mock<IAuthorizationService>();
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();
            var controller = new NewsController(context, fakeUserManager.Object, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);

            //Act
            var result = await controller.CreateComment(newsId, commentText, null);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateComment_ReplyCommentNotFound_ReturnsNotFound()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var newsId = new Guid("A0AA3082-2510-41B6-7267-08DC5A777836");
            var replyCommentId = 12345;
            const string commentText = "Добавлен новый тестовый комментарий";

            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeAuthService = new Mock<IAuthorizationService>();
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();
            var controller = new NewsController(context, fakeUserManager.Object, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);

            //Act
            var result = await controller.CreateComment(newsId, commentText, replyCommentId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateComment_ModelStateIsInvalid_ReturnsBadRequest()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var newsId = new Guid("A0AA3082-2510-41B6-7267-08DC5A777836");
            const string commentText = "Aa";
            const string ExpectedErrorMessage = "The field Text must be a string with a minimum length of 2 and a maximum length of 500.";

            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeAuthService = new Mock<IAuthorizationService>();
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();
            var controller = new NewsController(context, fakeUserManager.Object, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);
            controller.ModelState.AddModelError("Text", ExpectedErrorMessage);

            //Act
            var result = await controller.CreateComment(newsId, commentText, null);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreateComment_ModelStateIsValid_ReturnsComment()
        {
            // Arrange
            using var context = Fixture.CreateContext();
            var newsId = new Guid("A0AA3082-2510-41B6-7267-08DC5A777836");
            var todayDate = new DateTime(2024, 04, 24, 13, 13, 21, 746);
            const string commentText = "Добавлен новый тестовый комментарий";
            var user = new ApplicationUser
            {
                Id = "d077fbe6-2d31-4491-9bff-95dbb783acdd",
                UserName = "yyeeva"
            };
            context.Users.Attach(user);

            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(
                fakeUserStore.Object, null, null, null, null, null, null, null, null);
            fakeUserManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var fakeAuthService = new Mock<IAuthorizationService>();
            fakeAuthService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IAuthorizationRequirement[]>()))
                .ReturnsAsync(AuthorizationResult.Failed);
            var fakeDateTime = new Mock<IDateTime>();
            fakeDateTime.Setup(fakeDateTime => fakeDateTime.UtcNow).Returns(todayDate);
            var fakeFileManager = new Mock<IFileManager>();
            var controller = new NewsController(context, fakeUserManager.Object, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);

            // Act
            var result = await controller.CreateComment(newsId, commentText, null);

            // Assert
            Assert.IsType<JsonResult>(result);
            var comment = context.Comments.Single(b => b.Text == commentText);
            Assert.Equal(todayDate, comment.Created);
            Assert.Equal(user.UserName, comment.User.UserName);
            Assert.Equal(newsId, comment.News.Id);
        }

        [Fact]
        public async Task DeleteComment_NewsDoNotIncludeCommentId_ReturnsNotFound()
        {
            // Arrange
            using var context = Fixture.CreateContext();
            var id = new Guid("A0AA3082-2510-41B6-7267-08DC5A777836"); ;
            var commentId = 3;

            var fakeAuthService = new Mock<IAuthorizationService>();
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();
            var controller = new NewsController(context, _userManager, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);

            // Act
            var result = await controller.DeleteComment(id, commentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteComment_CommentDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            using var context = Fixture.CreateContext();
            var id = new Guid("A0AA3082-2510-41B6-7267-08DC5A777836");
            var commentId = 12345;

            var fakeAuthService = new Mock<IAuthorizationService>();
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();
            var controller = new NewsController(context, _userManager, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);

            // Act
            var result = await controller.DeleteComment(id, commentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteComment_UserUnauthorized_ReturnsForbid()
        {
            // Arrange
            using var context = Fixture.CreateContext();
            var id = new Guid("A0AA3082-2510-41B6-7267-08DC5A777836");
            var commentId = 1;

            var fakeAuthService = new Mock<IAuthorizationService>();
            fakeAuthService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IAuthorizationRequirement[]>()))
                .ReturnsAsync(AuthorizationResult.Failed);
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();

            var controller = new NewsController(context, _userManager, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);

            // Act
            var result = await controller.DeleteComment(id, commentId);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteComment_UserAuthorized_RedirectToActionResult()
        {
            // Arrange
            using var context = Fixture.CreateContext();
            var id = new Guid("A0AA3082-2510-41B6-7267-08DC5A777836");
            var commentId = 1;

            var fakeAuthService = new Mock<IAuthorizationService>();
            fakeAuthService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IAuthorizationRequirement[]>()))
                .ReturnsAsync(AuthorizationResult.Success);
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();

            var controller = new NewsController(context, _userManager, fakeAuthService.Object, fakeDateTime.Object, fakeFileManager.Object);

            // Act
            var result = await controller.DeleteComment(id, commentId);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
