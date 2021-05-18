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

            modelBuilder.Entity<Instapage>(builder =>
            {
                builder.ToTable("Instapages");
                builder.HasKey(o => o.InstapageId);
                builder.Property(o => o.InstapageId)
                    .ValueGeneratedNever()
                    .IsRequired();

                builder.Property(o => o.InstagramId)
                    .IsRequired();
                builder.Property(o => o.InstagramLogin)
                    .HasMaxLength(512)
                    .IsRequired();

                builder.Property(o => o.State)
                    .IsRequired();

                builder.Property(o => o.UserId)
                    .IsRequired(false);

                builder.Property(o => o.CreateDate)
                    .IsRequired();
                builder.Property(o => o.UpdateDate)
                    .IsRequired();

                builder.Property(o => o.AccessToken)
                    .IsRequired();

                builder.Property(o => o.Title)
                    .IsRequired(false);
                builder.Property(o => o.Description)
                    .IsRequired(false);

                builder.Property(o => o.ConcurrencyToken)
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Instapost>(builder =>
            {
                builder.ToTable("Instaposts");
                builder.HasKey(o => o.InstapostId);
                builder.Property(o => o.InstapostId)
                    .IsRequired();

                builder.Property(o => o.InstapageId)
                    .IsRequired();

                builder.Property(o => o.Title)
                    .HasMaxLength(128)
                    .IsRequired();

                builder.Property(o => o.Description);

                builder.Property(o => o.Price)
                    .IsRequired();

                builder.Property(o => o.Currency)
                    .HasMaxLength(128)
                    .IsRequired();
            });
        }
    }
}
