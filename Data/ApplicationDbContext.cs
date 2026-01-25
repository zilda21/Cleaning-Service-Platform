using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CleaningService.Web.Models;

namespace CleaningService.Web.Data
{

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}
    
         public DbSet<User> Users { get; set; }
             public DbSet<Booking> Bookings { get; set; }

    
}

}
