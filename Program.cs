using brevo_csharp.Client;
using EBest.Models;
using EBest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EBest
{
    public class Program
    {
        public static  void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // registering the DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // add identity services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;


                }
                ).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // email configurtion
            Configuration.Default.ApiKey.Add("api-key", builder.Configuration["BrevoSettings:ApiKey"]);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            // create a scope to seed the database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    DataBaseSeeder.SeedAsync(userManager, roleManager).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
                }
            }


            app.Run();
        }
    }
}
