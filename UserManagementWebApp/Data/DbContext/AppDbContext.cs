using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserManagementWebApp.Models;

namespace UserManagementWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity
                .ToTable("Users")
                .HasKey(c => c.Id);


            });
            base.OnModelCreating(modelBuilder);
        }

    }
}
