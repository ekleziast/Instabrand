using Instabrand.Domain.Registration;
using Microsoft.EntityFrameworkCore;

namespace Instabrand.Infrastructure.Registration
{
    public sealed class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

        internal DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("User");

                builder.HasKey(o => o.Id);
                builder.Property(o => o.Id)
                    .HasColumnName("UserId")
                    .ValueGeneratedNever()
                    .IsRequired();

                builder.Property(o => o.Email)
                    .HasMaxLength(512)
                    .IsRequired();

                builder.Property(co => co.EmailState)
                    .HasMaxLength(64)
                    .HasConversion<string>()
                    .IsRequired();

                builder.Property(o => o.CreateDate)
                    .IsRequired();

                builder.Property(o => o.PasswordHash)
                    .HasMaxLength(1024)
                    .IsRequired();

                builder.Property("_concurrencyToken")
                    .HasColumnName("ConcurrencyToken")
                    .IsConcurrencyToken();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
