using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Areas.Identity.Pages.Account;
using BadWolfTechnology.UnitTest.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BadWolfTechnology.UnitTest
{
    public class AccountControllerTest : IClassFixture<TestDatabaseFixture>
    {
        public TestDatabaseFixture Fixture { get; }

        public AccountControllerTest(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        [Theory]
        [InlineData("ivan29", "Логин ivan29 уже используется.")]
        public async void CheckUserNameAsync_WhenUserNameExists_JsonErrorMessage(string userName, string expected)
        {
            // Arrange
            using var context = Fixture.CreateContext();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeSingInManager = new Mock<SignInManager<ApplicationUser>>(fakeUserManager.Object,
                                                                             new HttpContextAccessor(),
                                                                             new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                                                                             null,
                                                                             null,
                                                                             null,
                                                                             null);
            var fakeLogger = new Mock<ILogger<AccountController>>();
            var controller = new AccountController(fakeSingInManager.Object, fakeUserManager.Object, fakeLogger.Object, context);

            // Act
            var result = await controller.CheckUserNameAsync(userName);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(expected, jsonResult.Value);
        }

        [Theory]
        [InlineData("s")]
        public async void CheckUserNameAsync_WhenUserNameIsInvalid_JsonFalse(string userName)
        {
            // Arrange
            using var context = Fixture.CreateContext();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeSingInManager = new Mock<SignInManager<ApplicationUser>>(fakeUserManager.Object,
                                                                             new HttpContextAccessor(),
                                                                             new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                                                                             null,
                                                                             null,
                                                                             null,
                                                                             null);
            var fakeLogger = new Mock<ILogger<AccountController>>();
            var controller = new AccountController(fakeSingInManager.Object, fakeUserManager.Object, fakeLogger.Object, context);
            controller.ModelState.AddModelError("Input.UserName", "The field UserName must be a string with a minimum length of 2 and a maximum length of 15.");

            // Act
            var result = await controller.CheckUserNameAsync(userName);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(false, jsonResult.Value);
        }

        [Theory]
        [InlineData("Nomad")]
        public async void CheckUserNameAsync_WhenUserNameIsAvailable_JsonTrue(string userName)
        {
            // Arrange
            using var context = Fixture.CreateContext();
            var fakeUserStore = new Mock<IUserStore<ApplicationUser>>();
            var fakeUserManager = new Mock<UserManager<ApplicationUser>>(fakeUserStore.Object, null, null, null, null, null, null, null, null);
            var fakeSingInManager = new Mock<SignInManager<ApplicationUser>>(fakeUserManager.Object,
                                                                             new HttpContextAccessor(),
                                                                             new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                                                                             null,
                                                                             null,
                                                                             null,
                                                                             null);
            var fakeLogger = new Mock<ILogger<AccountController>>();
            var controller = new AccountController(fakeSingInManager.Object, fakeUserManager.Object, fakeLogger.Object, context);

            // Act
            var result = await controller.CheckUserNameAsync(userName);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(true, jsonResult.Value);
        }
    }
}
