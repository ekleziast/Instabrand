using Instabrand.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Instabrand.Infrastructure.Users
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

                builder.HasKey(o => o.Id);
                builder.Property(o => o.Id)
                    .HasColumnName("UserId")
                    .ValueGeneratedNever()
                    .IsRequired();

                builder.Property(o => o.Email)
                    .HasMaxLength(512)
                    .IsRequired();

                builder.Property(o => o.CreateDate)
                    .IsRequired();

                builder.Property("_concurrencyToken")
                    .HasColumnName("ConcurrencyToken")
                    .IsConcurrencyToken();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
