namespace Imdb.DbContexts
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class ImdbContext : DbContext
    {
        public ImdbContext(DbContextOptions<ImdbContext> options)
            : base(options) { }

        public DbSet<NameBasics> NameBasics { get; set; }

        public DbSet<TitleAkas> TitleAkas { get; set; }

        public DbSet<TitleBasics> TitleBasics { get; set; }

        public DbSet<TitleCrew> TitleCrew { get; set; }

        public DbSet<TitleEpisode> TitleEpisodes { get; set; }

        public DbSet<TitlePrincipals> TitlePrincipals { get; set; }

        public DbSet<TitleRating> TitleRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnNameBasicsModelCreating(modelBuilder);
            OnTitleAkasModelCreating(modelBuilder);
            OnTitleBasicsModelCreating(modelBuilder);
            OnTitleCrewModelCreating(modelBuilder);
            OnTitleEpisodeModelCreating(modelBuilder);
            OnTitlePrincipalsModelCreating(modelBuilder);
            OnTitleRatingModelCreating(modelBuilder);
        }

        private static void OnNameBasicsModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NameBasics>()
                .ToTable("name.basics")
                .HasKey(k => new { k.NameId });

            modelBuilder.Entity<NameBasics>()
                .Property(p => p.Name)
                .IsRequired();
        }

        private static void OnTitleAkasModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TitleAkas>()
                .ToTable("title.akas")
                .HasKey(k => new { k.TitleId, k.Index });

            modelBuilder.Entity<TitleAkas>()
                .Property(p => p.Title)
                .IsRequired();
        }

        private static void OnTitleBasicsModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TitleBasics>()
                .ToTable("title.basics")
                .HasKey(k => new { k.TitleId });

            modelBuilder.Entity<TitleBasics>()
                .Property(p => p.TitleType)
                .IsRequired();

            modelBuilder.Entity<TitleBasics>()
                .Property(p => p.PrimaryTitle)
                .IsRequired();
        }

        private static void OnTitleCrewModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TitleCrew>()
                .ToTable("title.crew")
                .HasKey(k => new { k.TitleId });
        }

        private static void OnTitleEpisodeModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TitleEpisode>()
                .ToTable("title.episodes")
                .HasKey(k => new { k.TitleId });
        }

        private static void OnTitlePrincipalsModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TitlePrincipals>()
                .ToTable("title.principals")
                .HasKey(k => new { k.TitleId, k.Index });
        }

        private static void OnTitleRatingModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TitleRating>()
                .ToTable("title.ratings")
                .HasKey(k => new { k.TitleId });
        }
    }
}
