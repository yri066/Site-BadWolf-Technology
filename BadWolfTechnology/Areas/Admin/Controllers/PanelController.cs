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

        /// <summary>
        /// Список пользователей с ролями.
        /// </summary>
        /// <param name="Page">Номер страницы.</param>
        /// <returns>Страница списка пользователей с ролями.</returns>
        public async Task<ActionResult> RolesUsers(int Page = 1)
        {
            var defaultPageSize = 10;

            if (Page < 1)
            {
                Page = 1;
            }

            // Получить пользователей имеющие роли.
            var source = (from userRole in _context.UserRoles.OrderBy(user => user.UserId)
                          join user in _context.Users
                            on userRole.UserId equals user.Id
                          select new ApplicationUser
                          {
                              Id = user.Id,
                              UserName = user.UserName,
                          })
                         .Distinct()
                         .AsNoTracking();

            var pagination = await PaginatedList<ApplicationUser>.CreateAsync(source, Page, defaultPageSize);

            return View(pagination);
        }

        /// <summary>
        /// Профиль пользователя.
        /// </summary>
        /// <param name="id">Ид пользователя.</param>
        /// <returns>Страница профиля.</returns>
        public async Task<ActionResult> Profile(Guid id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesList = await _roleManager.Roles.ToListAsync();
            rolesList.RemoveAll(role => role.Name == SuperAdminRole);

            return View((user, rolesList, userRoles.ToList()));
        }

        /// <summary>
        /// Обновляет роли пользователя.
        /// </summary>
        /// <param name="id">Ид пользователя.</param>
        /// <param name="roles">Список назначенных ролей.</param>
        /// <returns>Страница профиля.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUserRolesAsync(Guid id, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, user, AdminOperations.UpdateRole);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            // Исключается роль SuperAdministrator
            roles.RemoveAll(role => role == SuperAdminRole);

            // получаем список ролей пользователя
            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
            userRoles.RemoveAll(role => role == SuperAdminRole);

            // получаем все роли
            var allRoles = _roleManager.Roles.ToList();
            allRoles.RemoveAll(role => role.Name == SuperAdminRole);

            // получаем список ролей, которые были добавлены
            var addedRoles = roles.Except(userRoles);

            // получаем роли, которые были удалены
            var removedRoles = userRoles.Except(roles);

            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);
            await _userManager.UpdateSecurityStampAsync(user);

            return RedirectToAction("Profile", new { id });
        }

        /// <summary>
        /// Блокирует пользователя.
        /// </summary>
        /// <param name="id">Ид пользователя.</param>
        /// <returns>Страница профиля.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BanUserAsync(Guid id)
        {
            var banLengthYears = 100;
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, user, AdminOperations.Ban);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            var endBanDate = DateTime.UtcNow.AddYears(banLengthYears);
            await ChangeUserBanAsync(user, endBanDate);

            return RedirectToAction("Profile", new { id });
        }

        /// <summary>
        /// Снимает блокировку с пользователя.
        /// </summary>
        /// <param name="id">Ид пользователя.</param>
        /// <returns>Страница профиля.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnbanningUserAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, user, AdminOperations.Ban);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            await ChangeUserBanAsync(user, null);

            return RedirectToAction("Profile", new { id });
        }

        /// <summary>
        /// Обновляет дату окончания блокировки.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="endBanDate">Дата окончания блокировки.</param>
        private async Task ChangeUserBanAsync(ApplicationUser user,DateTime? endBanDate)
        {
            user.LockoutEnd = endBanDate;

            await _userManager.UpdateAsync(user);
            await _userManager.UpdateSecurityStampAsync(user);
        }
    }
}
