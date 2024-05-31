using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Authorization.Admin;
using BadWolfTechnology.Authorization.Comment;
using BadWolfTechnology.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BadWolfTechnology.Areas.Admin.Controllers
{
    /// <summary>
    /// Панель администратора.
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "SuperAdministrator, Administrator")]
    public class PanelController : Controller
    {
        private const string SuperAdminRole = "SuperAdministrator";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthorizationService _authorizationService;

        public PanelController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Панель администратора.
        /// </summary>
        /// <returns>Страница администратора со списком постов.</returns>
        public async Task<ActionResult> IndexAsync()
        {
            var posts = await _context.Posts
                .Where(p => EF.Property<string>(p, "Discriminator") == nameof(Post))
                .Select(p => new Post
                {
                    Id = p.Id,
                    Title = p.Title,
                })
                .AsNoTracking().ToListAsync();

            return View(posts);
        }
    }
}
