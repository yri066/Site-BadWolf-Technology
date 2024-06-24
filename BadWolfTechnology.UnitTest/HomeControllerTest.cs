using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Controllers;
using BadWolfTechnology.Data.Interfaces;
using BadWolfTechnology.Models;
using BadWolfTechnology.UnitTest.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BadWolfTechnology.UnitTest
{
    public class HomeControllerTest : IClassFixture<TestDatabaseFixture>
    {
        public TestDatabaseFixture Fixture { get; }
        public HomeControllerTest(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public async void EditPost_ModelStateIsValid_ReturnsRedirectToAction()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var postId = new Guid("B43D176A-6CDA-46AC-AAA4-7B8720D0393D");
            var actionName = "Index";
            var post = new PostEdit()
            {
                Title = "Главная страница",
                Text = "Информация заполняется администратором.",
                CodePage = "Index",
                IsView = true,
            };

            var fakeLoggerProduct = new Mock<ILogger<ProductsController>>();
            var fakeServiceProvider = new Mock<IServiceProvider>();
                fakeServiceProvider.Setup(x => x.GetService(typeof(ILogger<ProductsController>))).Returns(fakeLoggerProduct.Object);
            var fakeLogger = new Mock<ILogger<HomeController>>();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();
            var fakeAuthService = new Mock<IAuthorizationService>();

            var controller = new HomeController(context, fakeLogger.Object,
                                                         fakeUserManager.Object,
                                                         fakeDateTime.Object,
                                                         fakeFileManager.Object,
                                                         fakeServiceProvider.Object,
                                                         fakeAuthService.Object);

            //Act
            var result = await controller.EditAsync(postId, post, null);

            //Assert
            var resultRedirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(actionName, resultRedirect.ActionName);
            var postResult = context.Posts.Single(x => x.Id == postId);
            Assert.Equal(post.Title, postResult.Title);
        }

        [Fact]
        public async void EditPost_ModelStateIsInvalid_ReturnView()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var postId = new Guid("B43D176A-6CDA-46AC-AAA4-7B8720D0393D");
            var viewName = "/Views/Products/Edit.cshtml";
            var post = new PostEdit()
            {
                Title = "Главная страница",
                CodePage = "Index",
                IsView = true,
            };

            var fakeLoggerProduct = new Mock<ILogger<ProductsController>>();
            var fakeServiceProvider = new Mock<IServiceProvider>();
            fakeServiceProvider.Setup(x => x.GetService(typeof(ILogger<ProductsController>))).Returns(fakeLoggerProduct.Object);
            var fakeLogger = new Mock<ILogger<HomeController>>();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();
            var fakeAuthService = new Mock<IAuthorizationService>();

            var controller = new HomeController(context, fakeLogger.Object,
                                                         fakeUserManager.Object,
                                                         fakeDateTime.Object,
                                                         fakeFileManager.Object,
                                                         fakeServiceProvider.Object,
                                                         fakeAuthService.Object);
            controller.ModelState.AddModelError("Text", "Поле Текст обязательно.");

            //Act
            var result = await controller.EditAsync(postId, post, null);

            //Assert
            Assert.IsType<ViewResult>(result);
            var resultView = Assert.IsType<ViewResult>(result);
            Assert.Equal(viewName, resultView.ViewName);
        }

        [Fact]
        public async void EditPost_PostNotFound_ReturnsNotFound()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var postId = new Guid("7CE19DA6-088A-4571-6924-08DC71341B5D");
            var post = new PostEdit()
            {
                Title = "Главная страница",
                Text = "Информация заполняется администратором.",
                CodePage = "Index",
                IsView = true,
            };

            var fakeLoggerProduct = new Mock<ILogger<ProductsController>>();
            var fakeServiceProvider = new Mock<IServiceProvider>();
            fakeServiceProvider.Setup(x => x.GetService(typeof(ILogger<ProductsController>))).Returns(fakeLoggerProduct.Object);
            var fakeLogger = new Mock<ILogger<HomeController>>();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();
            var fakeAuthService = new Mock<IAuthorizationService>();

            var controller = new HomeController(context, fakeLogger.Object,
                                                         fakeUserManager.Object,
                                                         fakeDateTime.Object,
                                                         fakeFileManager.Object,
                                                         fakeServiceProvider.Object,
                                                         fakeAuthService.Object);

            //Act
            var result = await controller.EditAsync(postId, post, null);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
