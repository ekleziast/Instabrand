using Microsoft.EntityFrameworkCore;

namespace Instabrand.Queries.Infrastructure.Users
{
    public sealed class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions options) : base(options) { }

        internal DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("User");

                builder.HasKey(o => o.UserId);
                builder.Property(o => o.UserId)
                    .ValueGeneratedNever()
                    .IsRequired();

                builder.Property(o => o.Email)
                    .HasMaxLength(512)
                    .IsRequired();
                builder.HasIndex(o => o.Email)
                    .IsUnique();

                builder.Property(co => co.EmailState)
                    .HasConversion<string>()
                    .HasMaxLength(64)
                    .IsRequired();

                builder.Property(o => o.CreateDate)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
