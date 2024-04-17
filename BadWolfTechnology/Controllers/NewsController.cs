using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Data;
using BadWolfTechnology.Data.Interfaces;
using BadWolfTechnology.Data.Services;
using BadWolfTechnology.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BadWolfTechnology.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDateTime _dateTime;

        public NewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDateTime dateTime)
        {
            _context = context;
            _userManager = userManager;
            _dateTime = dateTime;
        }

        [BindProperty]
        public NewsEdit Input { get; set; }


        // GET: NewsController
        public async Task<ActionResult> Index(int Page)
        {
            int defaultPageSize = 4;

            if (Page < 1)
            {
                Page = 1;
            }

            var source = _context.News.Select(news => new News
            {
                Id = news.Id,
                Title = news.Title,
                ImageName = news.ImageName,
                Text = news.Text,
                CommentCount = news.Comments.Count,
                IsView = news.IsView,
                IsDelete = news.IsDelete,
                Created = news.Created
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

        // GET: NewsController/Details/5
        [Route("News/{id:guid}")]
        public async Task<ActionResult> Details(Guid id)
        {
            var news = await _context.News.Include(news => news.Comments).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (news is null || news.IsDelete)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: NewsController/Create
        public ActionResult Create()
        {
            Input = new NewsEdit();
            return View("Edit", Input);
        }

        // POST: NewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormFile? image)
        {
            return await SaveNewsAsync(image);
        }

        // GET: NewsController/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            var news = await _context.News.Where(news => !news.IsDelete)
                .AsNoTracking()
                .FirstOrDefaultAsync(news => news.Id == id);

            if (news == null)
            {
                return NotFound();
            }

            var model = new NewsEdit()
            {
                Title = news.Title,
                ImageName = news.ImageName,
                Text = news.Text,
                IsView = news.IsView
            };

            return View(model);
        }

        // POST: NewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Guid id, IFormFile? image)
        {
            return await SaveNewsAsync(image, id);
        }

        // GET: NewsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NewsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Сохранить новость.
        /// </summary>
        /// <param name="image">Передаваемый файл.</param>
        /// <param name="id">Идентификатор новости при редактировании.</param>
        /// <returns></returns>
        [NonAction]
        public async Task<ActionResult> SaveNewsAsync(IFormFile? image, Guid id = new Guid())
        {
            //Сохранить изображение во временную папку.
            if (image != null)
            {
                try
                {
                    Input.TempImageName = await FileManager.UploadImageAsync("Temp", Input.TempImageName, image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Input.TempImageName), ex.Message);
                }
            }

            if (!ModelState.IsValid)
            {
                return View("Edit", Input);
            }

            //Перенести изображение в основную папку.
            if (Input.TempImageName is not null)
            {
                try
                {
                    FileManager.MoveImage("Temp", "News", Input.TempImageName);
                    Input.ImageName = Input.TempImageName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Input.TempImageName), ex.Message);
                    return View("Edit", Input);
                }
            }

            if(id == default)
            {
                var news = new News()
                {
                    Title = Input.Title,
                    ImageName = Input.ImageName,
                    Text = Input.Text,
                    IsView = Input.IsView,
                    Created = _dateTime.UtcNow
                };

                await _context.AddAsync(news);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { news.Id });
            }
            else
            {
                var news = await _context.News.FirstOrDefaultAsync(news => news.Id == id);

                if(news == null)
                {
                    return NotFound();
                }

                news.Title = Input.Title;
                news.Text = Input.Text;
                news.ImageName = Input.ImageName;
                news.IsView = Input.IsView;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { news.Id });
            }
        }
    }
}
