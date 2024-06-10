using AspNetCore.ReCaptcha;
using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Authorization.Admin;
using BadWolfTechnology.Data;
using BadWolfTechnology.Data.Interfaces;
using BadWolfTechnology.Data.Services;
using BadWolfTechnology.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace BadWolfTechnology
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("settings.json");
            builder.Configuration.Bind(OrganizationInfo.Position, new OrganizationInfo());
            var emailConfig = builder.Configuration.GetSection(EmailConfiguration.Position).Get<EmailConfiguration>();

            // Add services to the container.
            var provider = builder.Configuration.GetValue("Provider", "SqlServer");

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => _ = provider switch
                {
                    "SqlServer" => _ = options.UseSqlServer(
                        builder.Configuration.GetConnectionString("SqlServerConnection"),
                        x => x.MigrationsAssembly("SqlServerMigrations")),

                    "PostgreSQL" => _ = options.UseNpgsql(
                        builder.Configuration.GetConnectionString("PostgreSQLConnection"),
                        x => x.MigrationsAssembly("PostgreSQLMigrations")),

                    _ => throw new Exception($"Unsupported provider: {provider}")
                });

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();// Убрать при публикации.

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Lockout.AllowedForNewUsers = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddErrorDescriber<RussianIdentityErrorDescriber>();
            builder.Services.AddControllersWithViews();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
                options.Cookie.HttpOnly = true;
                options.LoginPath = new PathString("/Index");
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            builder.Services.Configure<SecurityStampValidatorOptions>(o => o.ValidationInterval = TimeSpan.FromMinutes(1));

            builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptcha"));
            builder.Services.AddSingleton<IEmailConfiguration>(emailConfig);
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddSingleton<IDateTime, SystemDateTime>();
            builder.Services.AddTransient<IFileManager, FileManager>();

            builder.Services.AddScoped<IAuthorizationHandler, CommentUserAuthorizationHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, CommentManagerAdministratorAuthorizationHandler>();

            builder.Services.AddSingleton<IAuthorizationHandler, NewsManagerAuthorizationHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, NewsAdministratorAuthorizationHandler>();

            builder.Services.AddSingleton<IAuthorizationHandler, ProductAdministratorAuthorizationHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, ProductManagerAuthorizationHandler>();

            builder.Services.AddSingleton<IAuthorizationHandler, PostAdministratorAuthorizationHandler>();

            builder.Services.AddScoped<IAuthorizationHandler, AdminAdministratorAuthorizationHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, AdminSuperAdministratorAuthorizationHandler>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "admin_area",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.MapRazorPages();

            app.Run();
        }
    }
}