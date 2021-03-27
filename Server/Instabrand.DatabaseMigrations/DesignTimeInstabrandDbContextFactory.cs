using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Instabrand.DatabaseMigrations
{
    public sealed class DesignTimeStreamingDbContextFactory : IDesignTimeDbContextFactory<InstabrandDbContext>
    {
        public InstabrandDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<InstabrandDbContext>();
            optionsBuilder.UseNpgsql("migrations");
            return new InstabrandDbContext(optionsBuilder.Options);
        }
    }
}
