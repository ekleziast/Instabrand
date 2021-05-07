using Instabrand.Domain.Instapage;
using Microsoft.EntityFrameworkCore;

namespace Instabrand.Infrastructure.Instapages
{
    public sealed class InstapagesDbContext : DbContext
    {
        public InstapagesDbContext(DbContextOptions options) : base(options) { }

        internal DbSet<Instapage> Instapages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instapage>(builder =>
            {
                builder.ToTable("Instapages");
                builder.HasKey(o => o.Id);
                builder.Property(o => o.Id)
                    .HasColumnName("InstapageId")
                    .ValueGeneratedNever()
                    .IsRequired();

                builder.Property(o => o.InstagramId)
                    .IsRequired();
                builder.Property(o => o.InstagramLogin)
                    .HasMaxLength(512)
                    .IsRequired();

                builder.Property(o => o.State)
                    .HasConversion<string>()
                    .IsRequired();

                builder.Property(o => o.UserId)
                    .IsRequired(false);

                builder.Property(o => o.CreateDate)
                    .IsRequired();
                builder.Property(o => o.UpdateDate)
                    .IsRequired();

                builder.Property(o => o.AccessToken)
                    .IsRequired();
            });
        }
    }
}
