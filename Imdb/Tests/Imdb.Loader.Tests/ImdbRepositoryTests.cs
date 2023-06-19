using Imdb.Common.DbContexts;
using Imdb.Common.DbContexts.Utilities;
using Imdb.Common.Models;
using Imdb.Loader.Models;
using Imdb.Loader.Providers;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

using Xunit;

namespace Imdb.Loader.Tests;

public class ImdbRepositoryTests : IDisposable
{
    private readonly string workingDirectory;

    public ImdbRepositoryTests()
    {
        workingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(workingDirectory);
    }

    public void Dispose()
    {
        Directory.Delete(workingDirectory, true);
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task ImdbFilesAreLoadedProperly()
    {
        using var context = GetImdbContext();
        var databaseSettings = GetDatabaseSettings();
        var imdbFiles = GetImdbFiles();

        var repository = new ImdbRepository(context, Options.Create(databaseSettings), null);

        await repository.UpdateDatabase(imdbFiles);

        var nameBasics = await context.NameBasics.OrderBy(o => o.NameId).ToListAsync();
        AssertNameBasics(nameBasics);

        var titleAkas = await context.TitleAkas.OrderBy(o => o.TitleId).ThenBy(o => o.Index).ToListAsync();
        AssertTitleAkas(titleAkas);

        var titleBasics = await context.TitleBasics.OrderBy(o => o.TitleId).ToListAsync();
        AssertTitleBasics(titleBasics);

        var titleCrew = await context.TitleCrew.OrderBy(o => o.TitleId).ToListAsync();
        AssertTitleCrew(titleCrew);

        var titleEpisodes = await context.TitleEpisodes.OrderBy(o => o.TitleId).ToListAsync();
        AssertTitleEpisodes(titleEpisodes);

        var titlePrincipals = await context.TitlePrincipals.OrderBy(o => o.TitleId).ThenBy(o => o.Index).ToListAsync();
        AssertTitlePrincipals(titlePrincipals);

        var titleRatings = await context.TitleRatings.OrderBy(o => o.TitleId).ToListAsync();
        AssertTitleRatings(titleRatings);
    }

    private static void AssertNameBasics(List<NameBasics> nameBasics)
    {
        Assert.Equal(2, nameBasics.Count);

        Assert.Equal<uint>(1, nameBasics[0].NameId);
        Assert.Equal("Fred Astaire", nameBasics[0].Name);
        Assert.Equal<uint?>(1899, nameBasics[0].BirthYear);
        Assert.Equal<uint?>(1987, nameBasics[0].DeathYear);
        Assert.Equal("soundtrack,actor,miscellaneous", nameBasics[0].Professions);
        Assert.Equal("50419,45537,72308,53137", nameBasics[0].KnownForTitleIds);

        Assert.Equal<uint>(2, nameBasics[1].NameId);
        Assert.Equal("Lauren Bacall", nameBasics[1].Name);
        Assert.Equal<uint?>(1924, nameBasics[1].BirthYear);
        Assert.Equal<uint?>(2014, nameBasics[1].DeathYear);
        Assert.Equal("actress,soundtrack", nameBasics[1].Professions);
        Assert.Equal("75213,37382,117057,38355", nameBasics[1].KnownForTitleIds);
    }

    private static void AssertTitleAkas(List<TitleAkas> titleAkas)
    {
        Assert.Equal(2, titleAkas.Count);

        Assert.Equal<uint>(1, titleAkas[0].TitleId);
        Assert.Equal<uint>(1, titleAkas[0].Index);
        Assert.Equal("Carmencita - spanyol tanc", titleAkas[0].Title);
        Assert.Equal("HU", titleAkas[0].Region);
        Assert.Equal("hu", titleAkas[0].Language);
        Assert.Equal("imdbDisplay", titleAkas[0].Types);
        Assert.Null(titleAkas[0].Attributes);
        Assert.Equal(false, titleAkas[0].IsOriginalTitle);

        Assert.Equal<uint>(1, titleAkas[1].TitleId);
        Assert.Equal<uint>(2, titleAkas[1].Index);
        Assert.Equal("Carmencita", titleAkas[1].Title);
        Assert.Equal("DE", titleAkas[1].Region);
        Assert.Null(titleAkas[1].Language);
        Assert.Null(titleAkas[1].Types);
        Assert.Equal("literal title", titleAkas[1].Attributes);
        Assert.Equal(false, titleAkas[1].IsOriginalTitle);
    }

    private static void AssertTitleBasics(List<TitleBasics> titleBasics)
    {
        Assert.Equal(2, titleBasics.Count);

        Assert.Equal<uint>(1, titleBasics[0].TitleId);
        Assert.Equal("short", titleBasics[0].TitleType);
        Assert.Equal("Carmencita", titleBasics[0].PrimaryTitle);
        Assert.Equal("Carmencita", titleBasics[0].OriginalTitle);
        Assert.Equal(false, titleBasics[0].IsAdult);
        Assert.Equal<uint?>(1894, titleBasics[0].StartYear);
        Assert.Equal<uint?>(1895, titleBasics[0].EndYear);
        Assert.Equal<uint?>(1, titleBasics[0].RuntimeMinutes);
        Assert.Equal("Documentary,Short", titleBasics[0].Genres);

        Assert.Equal<uint>(2, titleBasics[1].TitleId);
        Assert.Equal("short", titleBasics[1].TitleType);
        Assert.Equal("Le clown et ses chiens", titleBasics[1].PrimaryTitle);
        Assert.Equal("Le clown et ses chiens", titleBasics[1].OriginalTitle);
        Assert.Equal(false, titleBasics[1].IsAdult);
        Assert.Equal<uint?>(1892, titleBasics[1].StartYear);
        Assert.Null(titleBasics[1].EndYear);
        Assert.Equal<uint?>(5, titleBasics[1].RuntimeMinutes);
        Assert.Equal("Animation,Short", titleBasics[1].Genres);
    }

    private static void AssertTitleCrew(List<TitleCrew> titleCrew)
    {
        Assert.Equal(2, titleCrew.Count);

        Assert.Equal<uint>(1, titleCrew[0].TitleId);
        Assert.Equal("5690", titleCrew[0].DirectorNameIds);
        Assert.Null(titleCrew[0].WriterNameIds);

        Assert.Equal<uint>(2, titleCrew[1].TitleId);
        Assert.Equal("721526", titleCrew[1].DirectorNameIds);
        Assert.Equal("721526", titleCrew[1].WriterNameIds);
    }

    private static void AssertTitleEpisodes(List<TitleEpisode> titleEpisodes)
    {
        Assert.Equal(2, titleEpisodes.Count);

        Assert.Equal<uint>(41951, titleEpisodes[0].TitleId);
        Assert.Equal<uint>(41038, titleEpisodes[0].ParentTitleId);
        Assert.Equal<uint?>(1, titleEpisodes[0].SeasonNumber);
        Assert.Equal<uint?>(9, titleEpisodes[0].EpisodeNumber);

        Assert.Equal<uint>(42816, titleEpisodes[1].TitleId);
        Assert.Equal<uint>(989125, titleEpisodes[1].ParentTitleId);
        Assert.Equal<uint?>(1, titleEpisodes[1].SeasonNumber);
        Assert.Equal<uint?>(17, titleEpisodes[1].EpisodeNumber);
    }

    private static void AssertTitlePrincipals(List<TitlePrincipals> titlePrincipals)
    {
        Assert.Equal(2, titlePrincipals.Count);

        Assert.Equal<uint>(1, titlePrincipals[0].TitleId);
        Assert.Equal<uint>(1, titlePrincipals[0].Index);
        Assert.Equal<uint>(1588970, titlePrincipals[0].NameId);
        Assert.Equal("self", titlePrincipals[0].Category);
        Assert.Null(titlePrincipals[0].Job);
        Assert.Equal("Self", titlePrincipals[0].Characters);

        Assert.Equal<uint>(1, titlePrincipals[1].TitleId);
        Assert.Equal<uint>(2, titlePrincipals[1].Index);
        Assert.Equal<uint>(5690, titlePrincipals[1].NameId);
        Assert.Equal("director", titlePrincipals[1].Category);
        Assert.Equal("director of photography", titlePrincipals[1].Job);
        Assert.Null(titlePrincipals[1].Characters);
    }

    private static void AssertTitleRatings(List<TitleRating> titleRatings)
    {
        Assert.Equal(2, titleRatings.Count);

        Assert.Equal<uint>(1, titleRatings[0].TitleId);
        Assert.Equal(5.7, titleRatings[0].AverageRating);
        Assert.Equal<uint>(1974, titleRatings[0].NumberOfVotes);

        Assert.Equal<uint>(2, titleRatings[1].TitleId);
        Assert.Equal(5.8, titleRatings[1].AverageRating);
        Assert.Equal<uint>(264, titleRatings[1].NumberOfVotes);
    }

    private static DatabaseSettings GetDatabaseSettings() => new()
    {
        BatchSize = 100000,
        VacuumDatabase = false,
    };

    private ImdbContext GetImdbContext() => new(GetImdbContextOptions());

    private DbContextOptions<ImdbContext> GetImdbContextOptions() =>
        new DbContextOptionsBuilder<ImdbContext>()
            .UseSqlite($@"Data Source={workingDirectory}/imdb.db3;Pooling=False;")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll) //NoTracking
            .ReplaceService<IRelationalCommandBuilderFactory, SqliteRelationalCommandBuilderFactory>()
            .Options;

    private static ImdbFiles GetImdbFiles() => new()
    {
        NameBasics = @"ImdbFiles\name.basics.tsv",
        TitleAkas = @"ImdbFiles\title.akas.tsv",
        TitleBasics = @"ImdbFiles\title.basics.tsv",
        TitleCrew = @"ImdbFiles\title.crew.tsv",
        TitleEpisode = @"ImdbFiles\title.episode.tsv",
        TitlePrincipals = @"ImdbFiles\title.principals.tsv",
        TitleRatings = @"ImdbFiles\title.ratings.tsv",
    };
}