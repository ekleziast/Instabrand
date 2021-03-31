using Instabrand.Domain.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.Authentication
{
    public sealed class AuthenticationDbContext : DbContext
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options) { }

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

                builder.Property(o => o.PasswordHash)
                    .HasMaxLength(1024)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
