using Microsoft.EntityFrameworkCore;
using EBest.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EBest.Services
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext( DbContextOptions options ):base(options)
        {

        }
        public DbSet<Product> Products { set; get; }
        public DbSet<Order> Orders { set; get; }

    }
}
