using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Controllers;
using BadWolfTechnology.Data;
using BadWolfTechnology.Data.Interfaces;
using BadWolfTechnology.Models;
using BadWolfTechnology.UnitTest.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BadWolfTechnology.UnitTest
{
    public class ProductControllerTest : IClassFixture<TestDatabaseFixture>
    {
        public TestDatabaseFixture Fixture { get; }
        public ProductControllerTest(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public async void CreateProduct_ModelStateIsValid_ReturnsRedirectToAction()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var productId = new Guid("031D37CE-034D-4DAF-5FBF-08DC744C684D");
            var todayDate = new DateTime(2024, 05, 08);
            var actionName = "Details";
            var product = new PostEdit()
            {
                Title = "DynamicCoverVK",
                Text = "Динамическая обложка для сообществ и профилей ВКонтакте.",
                CodePage = "DynamicCoverVK",
                IsView = true,
            };


            var fakeLogger = new Mock<ILogger<ProductsController>>();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeDateTime = new Mock<IDateTime>();
                fakeDateTime.Setup(x => x.UtcNow).Returns(todayDate);
            var fakeFileManager = new Mock<IFileManager>();

            var controller = new ProductsController(context, fakeUserManager.Object,
                                                             fakeDateTime.Object,
                                                             fakeFileManager.Object,
                                                             fakeLogger.Object);
            controller.InputModel = product;

            //Act
            var result = await controller.CreateAsync(null);

            //Assert
            var resultRedirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(actionName, resultRedirect.ActionName);
            var productResult = context.Products.Single(x => x.Title == product.Title);
            Assert.Equal(product.Text, productResult.Text);
            Assert.Equal(todayDate, productResult.Created);
        }

        [Fact]
        public async void CreatePost_ModelStateIsInvalid_ReturnView()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var todayDate = new DateTime(2024, 05, 08);
            var viewName = "Edit";
            var product = new PostEdit()
            {
                Title = "DynamicCoverVK",
                CodePage = "DynamicCoverVK",
                IsView = true
            };
            var fakeLogger = new Mock<ILogger<ProductsController>>();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeDateTime = new Mock<IDateTime>();
                fakeDateTime.Setup(x => x.UtcNow).Returns(todayDate);
            var fakeFileManager = new Mock<IFileManager>();

            var controller = new ProductsController(context, fakeUserManager.Object,
                                                             fakeDateTime.Object,
                                                             fakeFileManager.Object,
                                                             fakeLogger.Object);
            controller.InputModel = product;
            controller.ModelState.AddModelError("Text", "Поле Текст обязательно.");

            //Act
            var result = await controller.CreateAsync(null);

            //Assert
            Assert.IsType<ViewResult>(result);
            var resultView = Assert.IsType<ViewResult>(result);
            Assert.Equal(viewName, resultView.ViewName);
        }

        [Fact]
        public async void EditProduct_ModelStateIsValid_ReturnView()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var productId = new Guid("031D37CE-034D-4DAF-5FBF-08DC744C684D");
            var todayDate = new DateTime(2024, 05, 08);
            var actionName = "Details";
            var product = new PostEdit()
            {
                Title = "DynamicCoverVK",
                Text = "Динамическая обложка для сообществ и профилей ВКонтакте.",
                CodePage = "DynamicCoverVK",
                IsView = true,
            };

            
            var fakeLogger = new Mock<ILogger<ProductsController>>();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();

            var controller = new ProductsController(context, fakeUserManager.Object,
                                                             fakeDateTime.Object,
                                                             fakeFileManager.Object,
                                                             fakeLogger.Object);
            controller.InputModel = product;

            //Act
            var result = await controller.EditAsync(productId, null);

            //Assert
            var resultRedirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(actionName, resultRedirect.ActionName);
            var productResult = context.Products.Single(x => x.Id == productId);
            Assert.Equal(product.Title, productResult.Title);
        }

        [Fact]
        public async void EditProduct_ModelStateIsInvalid_ReturnView()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var productId = new Guid("031D37CE-034D-4DAF-5FBF-08DC744C684D");
            var todayDate = new DateTime(2024, 05, 08);
            var viewName = "Edit";
            var product = new PostEdit()
            {
                Title = "DynamicCoverVK",
                CodePage = "DynamicCoverVK",
                IsView = true,
            };
            var fakeLogger = new Mock<ILogger<ProductsController>>();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();

            var controller = new ProductsController(context, fakeUserManager.Object,
                                                             fakeDateTime.Object,
                                                             fakeFileManager.Object,
                                                             fakeLogger.Object);
            controller.InputModel = product;
            controller.ModelState.AddModelError("Text", "Поле Текст обязательно.");

            //Act
            var result = await controller.EditAsync(productId, null);

            //Assert
            Assert.IsType<ViewResult>(result);
            var resultView = Assert.IsType<ViewResult>(result);
            Assert.Equal(viewName, resultView.ViewName);
        }

        [Fact]
        public async void EditProduct_ProductNotFound_ReturnsNotFound()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var productId = new Guid("7CE19DA6-088A-4571-6924-08DC71341B5D");
            var product = new PostEdit()
            {
                Title = "DynamicCoverVK",
                Text = "Динамическая обложка для сообществ и профилей ВКонтакте.",
                CodePage = "DynamicCoverVK",
                IsView = true,
            };


            var fakeLogger = new Mock<ILogger<ProductsController>>();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();

            var controller = new ProductsController(context, fakeUserManager.Object,
                                                             fakeDateTime.Object,
                                                             fakeFileManager.Object,
                                                             fakeLogger.Object);
            controller.InputModel = product;

            //Act
            var result = await controller.EditAsync(productId, null);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ProductDoesNotExist_ReturnsNotFoundAsync()
        {
            //Arrange
            var context = Fixture.CreateContext();
            var productId = new Guid("7CE19DA6-088A-4571-6924-08DC71341B5D");

            var fakeLogger = new Mock<ILogger<ProductsController>>();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();

            var controller = new ProductsController(context, fakeUserManager.Object,
                                                             fakeDateTime.Object,
                                                             fakeFileManager.Object,
                                                             fakeLogger.Object);

            //Act

            var result = await controller.Delete(productId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ProductIsExist_ReturnsRedirectToActionAsync()
        {
            //Arrange
            var context = Fixture.CreateContext();
            var productId = new Guid("031D37CE-034D-4DAF-5FBF-08DC744C684D");
            var actionName = "Index";

            var fakeLogger = new Mock<ILogger<ProductsController>>();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeDateTime = new Mock<IDateTime>();
            var fakeFileManager = new Mock<IFileManager>();

            var controller = new ProductsController(context, fakeUserManager.Object,
                                                             fakeDateTime.Object,
                                                             fakeFileManager.Object,
                                                             fakeLogger.Object);

            //Act

            var result = await controller.Delete(productId);

            //Assert
            var resultRedirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(actionName, resultRedirect.ActionName);
        }
    }
}
