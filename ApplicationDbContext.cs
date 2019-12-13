using Microsoft.EntityFrameworkCore;
using Valeo.Bot.Data.Entities;

namespace Valeo.Bot
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<ValeoUser> Users { get; set; }

        public DbSet<Registration> Registrations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { 
            Database.Migrate();
        }
    }
}