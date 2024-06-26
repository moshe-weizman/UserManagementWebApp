using Microsoft.EntityFrameworkCore;

namespace UserManagementWebApp.Data
{
    public interface IAppDbContextFactory
    {
        public AppDbContext CreateDbContext();
    }
    public class AppDbContextFactory : IAppDbContextFactory
    {
        private readonly DbContextOptions<AppDbContext> options;

        public AppDbContextFactory(DbContextOptions<AppDbContext> options)
        {
            this.options = options;
        }

        public AppDbContext CreateDbContext()
        {
            return new AppDbContext(options);
        }
    }
}
