using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Data.Interfaces;
using BadWolfTechnology.Data;
using BadWolfTechnology.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BadWolfTechnology.Authorization.Product;

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
        private readonly IAuthorizationService _authorizationService;

        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDateTime dateTime, IFileManager fileManager, ILogger<ProductsController> logger, IAuthorizationService authorization)
        {
            _context = context;
            _userManager = userManager;
            _dateTime = dateTime;
            _fileManager = fileManager;
            _logger = logger;
            _authorizationService = authorization;
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

            var source = _context.Products.Select(product => new Product
            {
                Id = product.Id,
                Title = product.Title,
                ImageName = product.ImageName,
                Text = product.Text,
                IsView = product.IsView,
                IsDelete = product.IsDelete,
                Created = product.Created,
                CodePage = product.CodePage,
            });

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, new Product(), ProductOperations.ViewHidden);

            if (!isAuthorized.Succeeded)
            {
                source = source.Where(product => product.IsView);
            }

            source = source.Where(product => !product.IsDelete)
                .OrderByDescending(product => product.Created)
                .AsNoTracking();

            var pagination = await PaginatedList<IPublication>.CreateAsync(source, Page, defaultPageSize);

            if (pagination.TotalPages < (Page - 1))
            {
                return NotFound();
            }

            return View(pagination);
        }

        [Route("Products/{CodePage}")]
        [AllowAnonymous]
        public async Task<ActionResult> Details(string CodePage)
        {
            var source = _context.Products;

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, new Product(), ProductOperations.ViewHidden);

            if (!isAuthorized.Succeeded)
            {
                source = (DbSet<Product>)source.Where(product => product.IsView);
            }

            var product = await source.Where(prod => !prod.IsDelete).AsNoTracking().FirstOrDefaultAsync(prod => prod.CodePage == CodePage);

            if (product is null)
            {
                return NotFound();
            }

            ViewData["Title"] = product.Title;
            return View(product);
        }

        [Route("Products/Create")]
        [Authorize(Roles = "Administrator, ProductManager")]
        public ActionResult Create()
        {
            ViewData["ImageFolder"] = "Product";
            return View("Edit", new PostEdit());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Products/Create")]
        [Authorize(Roles = "Administrator, ProductManager")]
        public async Task<ActionResult> CreateAsync(IFormFile? image)
        {
            ViewData["ImageFolder"] = "Product";
            return await SaveProductAsync(image);
        }

        [Authorize(Roles = "Administrator, ProductManager")]
        public async Task<ActionResult> Edit(Guid id)
        {
            var product = await _context.Products.Where(prod => !prod.IsDelete).AsNoTracking().FirstOrDefaultAsync(prod => prod.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            var postEdit = new PostEdit(product);
            postEdit.CodePage = product.CodePage;
            postEdit.Id = id;

            ViewData["ImageFolder"] = "Product";
            return View(postEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, ProductManager")]
        public async Task<ActionResult> EditAsync(Guid id, IFormFile? image)
        {
            InputModel.Id = id;
            ViewData["ImageFolder"] = "Product";
            return await SaveProductAsync(image, id);
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

        private async Task<ActionResult> SaveProductAsync(IFormFile? image, Guid id = new Guid())
        {
            await SavePostImageInTempFolderAsync(InputModel, image);
            
            if(!ModelState.IsValid)
            {
                return View("Edit", InputModel);
            }

            Product? product;

            var codes = await _context.CodePages.FirstOrDefaultAsync(code => code.CodePage == InputModel.CodePage);

            if (id == default)
            {
                if(codes != null)
                {
                    ModelState.AddModelError(nameof(InputModel.CodePage), "Данный код страницы уже занят.");
                    return View("Edit", InputModel);
                }

                product = new Product()
                {
                    CodePage = InputModel.CodePage,
                    Created = _dateTime.UtcNow
                };
                InputModel.PostUpdate(product);

                await _context.AddAsync(product);
                await _context.SaveChangesAsync();
            }
            else
            {
                product = await _context.Products.Where(prod => !prod.IsDelete).FirstOrDefaultAsync(prod => prod.Id == id);

                if (product == null)
                {
                    return NotFound();
                }
                Console.WriteLine(codes?.Id);
                Console.WriteLine(codes?.CodePage);

                if (codes != null && codes.Id != id)
                {
                    ModelState.AddModelError(nameof(InputModel.CodePage), "Код страницы занят другим продуктом.");
                    return View("Edit", InputModel);
                }

                InputModel.PostUpdate(product);
                product.CodePage = InputModel.CodePage;

                await _context.SaveChangesAsync();
            }

            var path = Path.Combine("Product", product.Id.ToString());
            MoveTempImageToPostFolderAsync(InputModel, path);

            InputModel.PostUpdate(product);
            await _context.SaveChangesAsync();

            if (!ModelState.IsValid)
            {
                InputModel.Id = product.Id;
                return View("Edit", InputModel);
            }

            return RedirectToAction(nameof(Details), new { product.CodePage });
        }

        /// <summary>
        /// Загружает изображения во временную папку.
        /// </summary>
        /// <param name="post">Модель с файлами.</param>
        /// <param name="image">Основное изображение.</param>
        [NonAction]
        public async Task SavePostImageInTempFolderAsync(PostEdit post, IFormFile? image)
        {
            //Сохранить изображение во временную папку.
            if (image != null)
            {
                try
                {
                    post.TempImageName = await _fileManager.UploadImageAsync("Temp", post.ImageName, image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(post.TempImageName), ex.Message);
                }
            }

            for (var contentNumber = 0; contentNumber < post.Contents.Count; contentNumber++)
            {
                for (var itemNumber = 0; itemNumber < post.Contents[contentNumber].Items.Count; itemNumber++)
                {
                    try
                    {
                        var item = post.Contents[contentNumber].Items[itemNumber];

                        if (item.File == null)
                        {
                            continue;
                        }

                        var tempName = await _fileManager.UploadImageAsync("Temp", item.ImageName, item.File);
                        post.Contents[contentNumber].Items[itemNumber].TempImageName = tempName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError($"Contents[{contentNumber}].Items[{itemNumber}].File", ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Переносит изображения из временной папки в основную.
        /// </summary>
        /// <param name="post">Модель.</param>
        /// <param name="path">Путь к папке в "wwwroot/image" </param>
        [NonAction]
        public void MoveTempImageToPostFolderAsync(PostEdit post, string path)
        {
            if (post.TempImageName is not null)
            {
                try
                {
                    _fileManager.MoveImage("Temp", path, post.TempImageName);
                    post.ImageName = post.TempImageName;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, nameof(ProductsController), DateTime.UtcNow);
                }
            }

            for (var contentNumber = 0; contentNumber < post.Contents.Count; contentNumber++)
            {
                for (var itemNumber = 0; itemNumber < post.Contents[contentNumber].Items.Count; itemNumber++)
                {
                    try
                    {
                        var item = post.Contents[contentNumber].Items[itemNumber];

                        if(item.TempImageName == null)
                        {
                            continue;
                        }

                        _fileManager.MoveImage("Temp", path, item.TempImageName);
                        post.Contents[contentNumber].Items[itemNumber].ImageName = item.TempImageName;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, nameof(ProductsController), DateTime.UtcNow);
                    }
                }
            }
        }
    }
}
