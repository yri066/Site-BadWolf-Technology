using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Data;
using BadWolfTechnology.Data.Interfaces;
using BadWolfTechnology.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BadWolfTechnology.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDateTime _dateTime;
        private readonly IFileManager _fileManager;

        public NewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDateTime dateTime, IFileManager fileManager)
        {
            _context = context;
            _userManager = userManager;
            _dateTime = dateTime;
            _fileManager = fileManager;
        }

        public NewsEdit Input { get; set; }


        // GET: NewsController
        public async Task<ActionResult> Index(int Page = 1)
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
                CommentCount = news.Comments.Where(comment => !comment.IsDeleted).Count(),
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
            var news = _context.News.Include(n => n.Comments).ThenInclude(comment => comment.Parent).FirstOrDefault(x => x.Id == id);

            if (news is null || news.IsDelete)
            {
                return NotFound();
            }

            foreach (var comment in news.Comments)
            {
                _context.Entry(comment).Reference(comment => comment.User).Load();
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
        public async Task<ActionResult> Create([Bind] NewsEdit Input, IFormFile? image)
        {
            this.Input = Input;
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
        public async Task<ActionResult> EditAsync(Guid id, [Bind] NewsEdit Input, IFormFile? image)
        {
            this.Input = Input;
            return await SaveNewsAsync(image, id);
        }

        // POST: NewsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var news = await _context.News.Include(news => news.Comments).FirstOrDefaultAsync(news => news.Id == id);

            if(news == null)
            {
                return NotFound();
            }

            news.IsDelete = true;
            foreach(var comment in news.Comments)
            {
                comment.IsDeleted = true;
            }

            await _context.SaveChangesAsync();

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("News/{id:guid}/CreateComment")]
        public async Task<ActionResult> CreateComment(Guid id, [Bind("Text")] [StringLength(maximumLength:500, MinimumLength = 2)]string Text, long? ReplyId)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var source = _context.News.Where(news => news.Id == id && !news.IsDelete);

            if(ReplyId is not null)
            {
                source = source.Include(news => news.Comments.Where(comment => comment.Id == ReplyId));
            }

            var news = await source.FirstOrDefaultAsync();

            if (news == null)
            {
                return NotFound();
            }

            if (ReplyId != null && news.Comments.IsNullOrEmpty())
            {
                return NotFound();
            }

            var comment = new Comment
            {
                Created = _dateTime.UtcNow,
                Parent = news.Comments.FirstOrDefault(),
                News = news,
                // Удалить пустые строки и повторяющиеся пробелы.
                Text = Regex.Replace(Text, @"^\s*(\r\n|\z)", "", RegexOptions.Multiline),
                User = await _userManager.GetUserAsync(User)
            };

            news.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Json(new { id = comment.Id, login = comment.User.UserName, comment.Text, date = comment.Created.ToString("yyyy-MM-ddTHH:mmZ") });
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
                    Input.TempImageName = await _fileManager.UploadImageAsync("Temp", Input.TempImageName, image);
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
                    _fileManager.MoveImage("Temp", "News", Input.TempImageName);
                    Input.ImageName = Input.TempImageName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Input.TempImageName), ex.Message);
                    return View("Edit", Input);
                }
            }

            if (id == default)
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
                var news = await _context.News.Where(news => !news.IsDelete).FirstOrDefaultAsync(news => news.Id == id);

                if (news == null)
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
