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
        }
    }
}
