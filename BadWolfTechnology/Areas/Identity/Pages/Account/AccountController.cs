using BadWolfTechnology.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace BadWolfTechnology.Areas.Identity.Pages.Account
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
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
        public async Task<IActionResult> Index(string returnPathName = null)
        {
            returnPathName ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                var userName = _userManager.FindByEmailAsync(Input.EmailOrUserName).Result?.UserName ?? Input.EmailOrUserName;
                var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnPathName);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnPathName, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                if (result.IsNotAllowed)
                {
                    return Json(new { error = "Почта не подтверждена."});
                }
                else
                {
                    return Json(new { error = "Неверный логин или пароль." });
                }
            }

            return Json(new { error = "Неверный логин или пароль." });
        }
    }
}
