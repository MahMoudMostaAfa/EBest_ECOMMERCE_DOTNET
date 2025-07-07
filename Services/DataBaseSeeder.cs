using EBest.Models;
using Microsoft.AspNetCore.Identity;

namespace EBest.Services
{
    public class DataBaseSeeder
    {

        public static async Task SeedAsync(UserManager<ApplicationUser>? userManager,
            RoleManager<IdentityRole>? roleManager
            )
        {

            if (userManager == null || roleManager == null)
            {
                Console.WriteLine("UserManager or RoleManager is null, skipping seeding.");
                return;
            }

            // Check if the admin role exists, if not create it
            var exists = await roleManager.RoleExistsAsync("admin");
            if (!exists)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
                Console.WriteLine("admin role created successfully .");
            }

            // Check if the seller role exists, if not create it
            var sellerExists = await roleManager.RoleExistsAsync("seller");
            if (!sellerExists)
            {
                await roleManager.CreateAsync(new IdentityRole("seller"));
                Console.WriteLine("seller role created successfully .");
            }

            // Check if the client role exists, if not create it
            var clientExists = await roleManager.RoleExistsAsync("client");
            if (!clientExists)
            {
                await roleManager.CreateAsync(new IdentityRole("client"));
                Console.WriteLine("client role created successfully .");
            }

            // Check if the admin user exists, if not create it

            var admin = await userManager.GetUsersInRoleAsync("admin");

            if (!admin.Any())
            {
                var applicationUser = new ApplicationUser()
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Address = "Admin Address",
                    Email = "admin@ebest.com",
                    UserName = "admin@ebest.com"

                
                };
                string password = "admin123";
                var created= await userManager.CreateAsync(applicationUser,password);
                if (created.Succeeded)
                {
                    Console.WriteLine($"admin created email : {applicationUser.Email}");
                    Console.WriteLine($"with password: {password}");
                    await userManager.AddToRoleAsync(applicationUser, "admin");
                }
            }


        }
    }
}