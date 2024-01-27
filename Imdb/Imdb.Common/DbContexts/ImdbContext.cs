using Imdb.Common.Models;

using Microsoft.EntityFrameworkCore;

namespace Imdb.Common.DbContexts;

public class ImdbContext(DbContextOptions<ImdbContext> options) : DbContext(options)
{
    public DbSet<NameBasics> NameBasics { get; set; }

    public DbSet<TitleAkas> TitleAkas { get; set; }

    public DbSet<TitleBasics> TitleBasics { get; set; }

    public DbSet<TitleCrew> TitleCrew { get; set; }

    public DbSet<TitleEpisode> TitleEpisodes { get; set; }

    public DbSet<TitlePrincipals> TitlePrincipals { get; set; }

    public DbSet<TitleRating> TitleRatings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NameBasics>(e =>
        {
            e.ToTable("name.basics");
            e.HasKey(k => new { k.NameId });
            e.Property(p => p.NameId)
                .ValueGeneratedNever();
            e.Property(p => p.Name)
                .IsRequired();
        });

        modelBuilder.Entity<TitleAkas>(e =>
        {
            e.ToTable("title.akas");
            e.HasKey(k => new { k.TitleId, k.Index });
            e.Property(p => p.Title)
                .IsRequired();
        });

        modelBuilder.Entity<TitleBasics>(e =>
        {
            e.ToTable("title.basics");
            e.HasKey(k => new { k.TitleId });
            e.Property(p => p.TitleId)
                .ValueGeneratedNever();
            e.Property(p => p.TitleType)
                .IsRequired();
            e.Property(p => p.PrimaryTitle)
                .IsRequired();
        });

        modelBuilder.Entity<TitleCrew>(e =>
        {
            e.ToTable("title.crew");
            e.HasKey(k => new { k.TitleId });
            e.Property(p => p.TitleId)
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<TitleEpisode>(e =>
        {
            e.ToTable("title.episodes");
            e.HasKey(k => new { k.TitleId });
            e.Property(p => p.TitleId)
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<TitlePrincipals>(e =>
        {
            e.ToTable("title.principals");
            e.HasKey(k => new { k.TitleId, k.Index });
        });

        modelBuilder.Entity<TitleRating>(e =>
        {
            e.ToTable("title.ratings");
            e.HasKey(k => new { k.TitleId });
            e.Property(p => p.TitleId)
                .ValueGeneratedNever();
        });
    }
}
