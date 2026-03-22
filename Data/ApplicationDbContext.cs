using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CleaningService.Models;

namespace CleaningService.Data
{

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}
    
         public new DbSet<User> Users { get; set; }
             public DbSet<Booking> Bookings { get; set; }

    
}

}
