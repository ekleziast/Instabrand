using Microsoft.EntityFrameworkCore;

namespace Instabrand.DatabaseMigrations
{
    public sealed class InstabrandDbContext : DbContext
    {
        public InstabrandDbContext(DbContextOptions<InstabrandDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
