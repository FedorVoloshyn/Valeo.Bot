using Microsoft.EntityFrameworkCore;
using ValeoBot.Data.Entities;

namespace ValeoBot
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { 
            Database.EnsureCreated();
        }
    }
}