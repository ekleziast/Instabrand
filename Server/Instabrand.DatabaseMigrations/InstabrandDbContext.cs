using Instabrand.DatabaseMigrations.Entities;
using Microsoft.EntityFrameworkCore;

namespace Instabrand.DatabaseMigrations
{
    public sealed class InstabrandDbContext : DbContext
    {
        public InstabrandDbContext(DbContextOptions<InstabrandDbContext> options) : base(options) { }

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
                    .HasMaxLength(64)
                    .IsRequired();

                builder.Property(o => o.CreateDate)
                    .IsRequired();

                builder.Property(o => o.PasswordHash)
                    .HasMaxLength(1024)
                    .IsRequired();

                builder.Property(co => co.ConcurrencyToken)
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<RefreshToken>(builder =>
            {
                builder.ToTable("RefreshToken");

                builder.HasKey(o => o.RefreshTokenId);
                builder.Property(o => o.RefreshTokenId)
                    .HasMaxLength(64)
                    .ValueGeneratedNever();

                builder.Property(o => o.UserId)
                    .IsRequired();
                builder.Property(o => o.Expire)
                    .IsRequired();
            });
        }
    }
}
