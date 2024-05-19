using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Data.Interfaces;
using BadWolfTechnology.Data;
using BadWolfTechnology.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BadWolfTechnology.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDateTime _dateTime;
        private readonly IFileManager _fileManager;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDateTime dateTime, IFileManager fileManager, ILogger<ProductsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _dateTime = dateTime;
            _fileManager = fileManager;
            _logger = logger;
        }

        [BindProperty]
        public PostEdit InputModel { get; set; }

        [Route("Products")]
        [AllowAnonymous]
        public async Task<ActionResult> IndexAsync(int Page = 1)
        {
            int defaultPageSize = 4;

            if (Page < 1)
            {
                Page = 1;
            }

            var source = _context.Products.Select(news => new Product
            {
                Id = news.Id,
                Title = news.Title,
                ImageName = news.ImageName,
                Text = news.Text,
                IsView = news.IsView,
                IsDelete = news.IsDelete,
                Created = news.Created,
                CodePage = news.CodePage,
            })
                .Where(news => news.IsView)
                .Where(news => !news.IsDelete)
                .OrderByDescending(news => news.Created)
                .AsNoTracking();

            var pagination = await PaginatedList<IPublication>.CreateAsync(source, Page, defaultPageSize);

            if (pagination.TotalPages < (Page - 1))
            {
                return NotFound();
            }

            return View(pagination);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Products/Delete")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var product = await _context.Products.Where(prod => !prod.IsDelete).FirstOrDefaultAsync(prod => prod.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            product.IsDelete = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
