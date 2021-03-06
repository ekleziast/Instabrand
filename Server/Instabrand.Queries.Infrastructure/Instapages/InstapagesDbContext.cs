﻿using Microsoft.EntityFrameworkCore;

namespace Instabrand.Queries.Infrastructure.Instapages
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
                builder.HasKey(o => o.InstapageId);
                builder.Property(o => o.InstapageId)
                    .ValueGeneratedNever()
                    .IsRequired();

                builder.Property(o => o.InstagramLogin)
                    .HasMaxLength(512)
                    .IsRequired();

                builder.Property(o => o.Vkontakte)
                    .HasMaxLength(1024)
                    .IsRequired(false);
                builder.Property(o => o.Telegram)
                    .HasMaxLength(1024)
                    .IsRequired(false);

                builder.Property(o => o.State)
                    .HasConversion<string>()
                    .IsRequired();

                builder.Property(o => o.CreateDate)
                    .IsRequired();

                builder.Property(o => o.Title)
                    .IsRequired(false);
                builder.Property(o => o.Description)
                    .IsRequired(false);

                builder.HasMany(instapage => instapage.Instaposts).WithOne()
                    .HasForeignKey(instapost => instapost.InstapageId)
                    .HasPrincipalKey(instapage => instapage.InstapageId);
            });

            modelBuilder.Entity<Instapost>(builder =>
            {
                builder.ToTable("Instaposts");
                builder.HasKey(o => o.InstapostId);
                builder.Property(o => o.InstapostId)
                    .ValueGeneratedNever()
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

                builder.Property(o => o.Timestamp)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
