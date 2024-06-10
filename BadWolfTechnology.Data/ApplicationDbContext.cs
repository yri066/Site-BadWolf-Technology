using BadWolfTechnology.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace BadWolfTechnology.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<News> News { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<PostCodePages> CodePages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>(x =>
            {
                x.Property(b => b.Title)
                    .HasMaxLength(100);

                x.Property(b => b.CodePage)
                    .HasMaxLength(50);

                x.HasIndex(b => b.CodePage);

                x.Property(b => b.Contents)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<Content>>(v, (JsonSerializerOptions)null),
                        new ValueComparer<IList<Content>>(
                            (c1, c2) => c1.Equals(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => (IList<Content>)c.ToList()));
            });

            builder.Entity<Product>(x =>
            {
                x.Property(b => b.Title)
                    .HasMaxLength(100);

                x.Property(b => b.CodePage)
                    .HasMaxLength(50);

                x.HasIndex(b => b.CodePage);

                x.Property(b => b.Contents)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<Content>>(v, (JsonSerializerOptions)null),
                        new ValueComparer<IList<Content>>(
                            (c1, c2) => c1.Equals(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => (IList<Content>)c.ToList()));
            });

            builder.Entity<Comment>(x =>
            {
                x.Property(b => b.Text)
                    .HasMaxLength(500);
            });

            builder.Entity<PostCodePages>(x =>
            {
                x.ToView("View_PostCodePages");
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "32098694-1c75-46d3-87a8-da162dab0335",
                Name = "SuperAdministrator",
                NormalizedName = "SUPERADMINISTRATOR"
            },
            new IdentityRole
            {
                Id = "874a001d-3ef4-49af-a579-844c9a1034b4",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            },
            new IdentityRole
            {
                Id = "114fcebd-a934-479c-aa42-48d5d84e0670",
                Name = "NewsManager",
                NormalizedName = "NEWSMANAGER"
            },
            new IdentityRole
            {
                Id = "4e9c0350-32c1-464f-b41a-2d0b595a0a8c",
                Name = "CommentManager",
                NormalizedName = "COMMENTMANAGER"
            },
            new IdentityRole
            {
                Id = "c91f0ea1-9279-46fb-bb9e-f70b0879a0c7",
                Name = "ProductManager",
                NormalizedName = "PRODUCTMANAGER"
            });

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "87f7d358-de81-415b-a498-b08e0cf90636",
                FirstName = "Admin",
                LastName = "Admin",
                BirthDate = new DateTime(1997, 3, 11),
                UserName = "Admin",
                NormalizedUserName = "Admin",
                Email = "admin@badwolf.tech",
                NormalizedEmail = "ADMIN@BADWOLF.TECH",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "12qwQ!")
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "32098694-1c75-46d3-87a8-da162dab0335",
                UserId = "87f7d358-de81-415b-a498-b08e0cf90636"
            },
            new IdentityUserRole<string>
            {
                RoleId = "874a001d-3ef4-49af-a579-844c9a1034b4",
                UserId = "87f7d358-de81-415b-a498-b08e0cf90636"
            });

            builder.Entity<Post>().HasData(new Post
            {
                Id = new Guid("b43d176a-6cda-46ac-aaa4-7b8720d0393d"),
                Title = "Главная",
                Text = "Информация заполняется администратором.",
                Created = new DateTime(2024, 05, 08),
                CodePage = "Index"
            });

            builder.Entity<Post>().HasData(new Post
            {
                Id = new Guid("7bda8276-759d-4bbb-b795-104b2d2c5235"),
                Title = "Конфиденциальность",
                Text = "Информация заполняется администратором.",
                Created = new DateTime(2024, 05, 08),
                CodePage = "Privacy"
            });

            builder.Entity<Post>().HasData(new Post
            {
                Id = new Guid("88494cd9-a407-4ce6-aff4-8e5e25a1df5a"),
                Title = "Cookies",
                Text = "Информация заполняется администратором.",
                Created = new DateTime(2024, 05, 08),
                CodePage = "Cookies"
            });
        }
    }
}
