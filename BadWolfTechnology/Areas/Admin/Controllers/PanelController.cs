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

        /// <summary>
        /// Список пользователей.
        /// </summary>
        /// <param name="Page">Номер страницы.</param>
        /// <returns>Страница списка пользователей.</returns>
        public async Task<ActionResult> UsersAsync(int Page = 1)
        {
            var defaultPageSize = 10;

            if (Page < 1)
            {
                Page = 1;
            }

            var source = _context.Users.Select(user => new ApplicationUser
            {
                Id = user.Id,
                UserName = user.UserName,
            }).AsNoTracking();

            var pagination = await PaginatedList<ApplicationUser>.CreateAsync(source, Page, defaultPageSize);

            if (pagination.TotalPages < (Page - 1))
            {
                return NotFound();
            }

            return View(pagination);
        }

        /// <summary>
        /// Заблокированные пользователи
        /// </summary>
        /// <param name="Page">Номер страницы.</param>
        /// <returns>Страница заблокированных пользователей.</returns>
        public async Task<ActionResult> BannedUsersAsync(int Page = 1)
        {
            var defaultPageSize = 10;

            if (Page < 1)
            {
                Page = 1;
            }

            var source = _context.Users.Select(user => new ApplicationUser
            {
                Id = user.Id,
                UserName = user.UserName,
                LockoutEnd = user.LockoutEnd,
            }).Where(user => user.LockoutEnd > DateTime.UtcNow).AsNoTracking();

            var pagination = await PaginatedList<ApplicationUser>.CreateAsync(source, Page, defaultPageSize);

            if (pagination.TotalPages < (Page - 1))
            {
                return NotFound();
            }

            return View(pagination);
        }

        /// <summary>
        /// Список комментариев.
        /// </summary>
        /// <param name="Page">Номер страницы.</param>
        /// <returns>Страница комментариев.</returns>
        public async Task<ActionResult> CommentsAsync(int Page = 1)
        {
            var defaultPageSize = 10;

            if (Page < 1)
            {
                Page = 1;
            }

            var source = _context.Comments
                .Include(comment => comment.User)
                .Include(comment => comment.News)
                .Where(comment => !comment.IsDeleted)
                .OrderByDescending(comment => comment.Id)
                .AsNoTracking();

            var pagination = await PaginatedList<Comment>.CreateAsync(source, Page, defaultPageSize);

            if (pagination.TotalPages < (Page - 1))
            {
                return NotFound();
            }

            return View(pagination);
        }
    }
}
