using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace BadWolfTechnology.Areas.Identity.Pages.Account
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDbContext _context;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<AccountController> logger, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100)]
            public string EmailOrUserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [StringLength(100)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        [HttpPost("/Account/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SingIn(string returnPathName = null)
        {
            returnPathName ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var user = await _userManager.FindByEmailAsync(Input.EmailOrUserName) ?? await _userManager.FindByNameAsync(Input.EmailOrUserName);

                if (user == null)
                {
                    return Json(new { error = "Неверный логин или пароль." });
                }

                var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

                if (!emailConfirmed)
                {
                    return Json(new { error = "Почта не подтверждена." });
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnPathName);
                }
                if (result.RequiresTwoFactor)
                {
                    return Json(new { url = "/Identity/Account/LoginWith2fa" });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return Json(new { error = "Аккаунт временно заблокирован.\n Попробуйте войти позже." });
                }
                if (result.IsNotAllowed)
                {
                    return Json(new { error = "Вход не разрешен."});
                }
            }

            return Json(new { error = "Неверный логин или пароль." });
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> CheckUserNameAsync([Bind(Prefix = "Input.UserName")][StringLength(maximumLength:15, MinimumLength = 2)] string UserName)
        {
            if(!ModelState.IsValid)
            {
                return Json(false);
            }

            if(!string.IsNullOrEmpty(await _context.Users.Select(user => user.NormalizedUserName).AsNoTracking().FirstOrDefaultAsync(username => username == UserName.ToUpper())))
            {
                return Json($"Логин {UserName} уже используется.");
            }

            return Json(true);
        }
    }
}
