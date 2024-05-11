using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BadWolfTechnology.UnitTest.Data
{
    public class TestDatabaseFixture
    {
        private const string ConnectionString = "Server=DESKTOP-OI8ARDG;Database=EFTestSample;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private static readonly object _lock = new();
        private static bool _dbInitialized;

        public TestDatabaseFixture()
        {
            lock (_lock)
            {
                if (_dbInitialized)
                    return;

                using (var context = CreateContext())
                {
                    AddData(context);
                }

                _dbInitialized = true;

            }
        }

        public ApplicationDbContext CreateContext()
            => new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(ConnectionString)
                .Options
        );

        private void AddData(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user1 = new ApplicationUser()
            {
                Id = "292b6072-7828-4d3d-bf27-c1aefadadf07",
                FirstName = "Георгий",
                LastName = "Леонов",
                BirthDate = new DateTime(1997, 3, 11),
                UserName = "mr.georgy",
                NormalizedUserName = "MR.GEORGY",
                Email = "cejexo5184@losvtn.com",
                NormalizedEmail = "CEJEXO5184@LOSVTN.COM",
                EmailConfirmed = true
            };
            user1.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user1, "12qwQ!");

            var user2 = new ApplicationUser()
            {
                Id = "f091ced6-84c0-4c0c-9d8d-6f4195ef1101",
                FirstName = "Иван",
                LastName = "Молчанов",
                BirthDate = new DateTime(2004, 7, 29),
                UserName = "ivan29",
                NormalizedUserName = "IVAN29",
                Email = "xicat57389@mfyax.com",
                NormalizedEmail = "XICAT57389@MFYAX.COM",
                EmailConfirmed = true
            };
            user2.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user2, "12qwQ!");

            var user3 = new ApplicationUser()
            {
                Id = "d077fbe6-2d31-4491-9bff-95dbb783acdd",
                FirstName = "Ева",
                LastName = "Румянцева",
                BirthDate = new DateTime(1997, 2, 12),
                UserName = "yyeeva",
                NormalizedUserName = "YYEEVA",
                Email = "ribaxaf359@nweal.com",
                NormalizedEmail = "RIBAXAF359@NWEAL.COM",
                EmailConfirmed = true
            };
            user3.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user3, "12qwQ!");

            var news1 = new News()
            {
                Id = new Guid("A0AA3082-2510-41B6-7267-08DC5A777836"),
                Title = "News 1",
                Text = "News 1",
                Created = new DateTime(2024, 04, 11, 22, 33, 54, 277)
            };
            var news1_comments1 = new Comment()
            {
                Text = "Первый комментарий!",
                Created = new DateTime(2024, 04, 19, 19, 23, 53, 189),
                User = user1,
                News = news1,
            };
            var news1_comments2 = new Comment()
            {
                Text = "Второй",
                Created = new DateTime(2024, 04, 19, 19, 24, 24, 937),
                User = user2,
                News = news1,
                Parent = news1_comments1
            };
            var news1_comments3 = new Comment()
            {
                Text = "третий:)",
                Created = new DateTime(2024, 04, 19, 19, 25, 43, 123),
                User = user3,
                News = news1,
                Parent = news1_comments2
            };
            var news1_comments4 = new Comment()
            {
                Text = "Последний!",
                Created = new DateTime(2024, 04, 19, 19, 32, 11, 103),
                User = user1,
                News = news1,
                Parent = news1_comments1
            };
            var news1_comments5 = new Comment()
            {
                Text = "Вдохновляющая новость",
                Created = new DateTime(2024, 04, 20, 8, 11, 32, 246),
                User = user3,
                News = news1,
            };
            news1.Comments = new List<Comment>() { news1_comments1, news1_comments2, news1_comments3, news1_comments4, news1_comments5 };

            var news2 = new News()
            {
                Id = new Guid("DC911216-A63A-4639-B96C-08DC5A7A21D0"),
                Title = "News 2",
                Text = "News 2",
                Created = new DateTime(2024, 04, 14, 06, 53, 48, 723)
            };

            var news3 = new News()
            {
                Id = new Guid("CA090C8C-7ED4-4726-429D-08DC5C4FA310"),
                Title = "News 3",
                Text = "News 3",
                Created = new DateTime(2024, 04, 22, 12, 51, 36, 064)
            };

            var news3_comments1 = new Comment()
            {
                Text = "Пост мне понравился",
                Created = new DateTime(2024, 04, 22, 13, 12, 36, 064),
                User = user1,
                News = news1,
            };
            var news3_comments2 = new Comment()
            {
                Text = "Давно хотел узнать об этом.",
                Created = new DateTime(2024, 04, 22, 13, 15, 43, 624),
                User = user2,
                News = news1,
                Parent = news1_comments1
            };
            var news3_comments3 = new Comment()
            {
                Text = "Ничего не понял!!!",
                Created = new DateTime(2024, 04, 23, 16, 1, 43, 824),
                User = user3,
                News = news1,
            };
            news3.Comments = new List<Comment>() { news3_comments1, news3_comments2, news3_comments3 };

            context.AddRange(user1, user2, user3);
            context.AddRange(news1, news2, news3);

            context.SaveChanges();
        }
    }
}
