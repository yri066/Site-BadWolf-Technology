using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Data;
using BadWolfTechnology.Data.Interfaces;
using BadWolfTechnology.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BadWolfTechnology.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDateTime _dateTime;
        private readonly IFileManager _fileManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public HomeController(ApplicationDbContext context,
                              ILogger<HomeController> logger,
                              UserManager<ApplicationUser> userManager,
                              IDateTime dateTime,
                              IFileManager fileManager,
                              IServiceProvider serviceProvider)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _dateTime = dateTime;
            _fileManager = fileManager;
            _serviceProvider = serviceProvider;
        }

        [Route("{CodePage?}")]
        [AllowAnonymous]
        public async Task<ActionResult> Index(string CodePage = "Index")
        {
            return await FindPageAsync(CodePage);
        }

        [Route("StatusCode/{CodeError:int}")]
        [AllowAnonymous]
        public new ActionResult StatusCode(int CodeError)
        {
            return View(CodeError);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(Guid id)
        {
            var post = await _context.Posts
                .Where(p => EF.Property<string>(p, "Discriminator") == nameof(Post))
                .FirstOrDefaultAsync(x => x.Id == id);

            if (post is null)
            {
                return NotFound();
            }

            ViewData["ImageFolder"] = "Post";
            ViewData["CodePage"] = false;
            return View("/Views/Products/Edit.cshtml", new PostEdit(post) { Id = post.Id, CodePage = post.CodePage });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> EditAsync(Guid id, PostEdit post, IFormFile? image)
        {
            ViewData["ImageFolder"] = "Post";
            ViewData["CodePage"] = false;
            return await SavePostAsync(post, image, id);
        }

        [Route("Privacy")]
        [AllowAnonymous]
        public async Task<ActionResult> PrivacyAsync()
        {
            ViewData["Layout"] = "_LayoutWithoutLogin";
            return await FindPageAsync("Privacy");
        }

        [Route("Cookies")]
        [AllowAnonymous]
        public async Task<ActionResult> CookiesAsync()
        {
            ViewData["Layout"] = "_LayoutWithoutLogin";
            return await FindPageAsync("Cookies");
        }

        private async Task<ActionResult> FindPageAsync(string CodePage, string view = "Index")
        {
            var post = await _context.Posts
                .Where(p => EF.Property<string>(p, "Discriminator") == nameof(Post))
                .FirstOrDefaultAsync(x => x.CodePage == CodePage);

            if (post is null || !post.IsView)
            {
                return NotFound();
            }

            ViewData["Title"] = post.Title;
            return View(view, post);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<ActionResult> SavePostAsync(PostEdit InputModel, IFormFile? image, Guid id = new Guid())
        {
            var logger = _serviceProvider.GetService(typeof(ILogger<ProductsController>)) as ILogger<ProductsController>;
            var productController = new ProductsController(_context, _userManager, _dateTime, _fileManager, logger);
            await productController.SavePostImageInTempFolderAsync(InputModel, image);

            if (!ModelState.IsValid)
            {
                return View("/Views/Products/Edit.cshtml", InputModel);
            }

            var post = await _context.Posts.FirstOrDefaultAsync(prod => prod.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            InputModel.PostUpdate(post);
            await _context.SaveChangesAsync();

            var path = Path.Combine("Post", post.Id.ToString());
            productController.MoveTempImageToPostFolderAsync(InputModel, path);

            InputModel.PostUpdate(post);
            await _context.SaveChangesAsync();

            if (!ModelState.IsValid)
            {
                InputModel.Id = post.Id;
                return View("/Views/Products/Edit.cshtml", InputModel);
            }

            return RedirectToAction(nameof(Index), new { post.CodePage });
        }
    }
}
