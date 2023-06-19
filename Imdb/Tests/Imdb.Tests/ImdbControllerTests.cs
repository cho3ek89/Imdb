using Imdb.Common.DbContexts;
using Imdb.Common.Models;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System.Net.Http.Json;
using System.Text.Json.Serialization;

using Xunit;

namespace Imdb.Tests;

public class ImdbControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> webApplicationFactory;

    public ImdbControllerTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        this.webApplicationFactory = webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<ImdbContext>();
                services.RemoveAll<DbContextOptions>();

                foreach (var option in services.Where(s => s.ServiceType.BaseType == typeof(DbContextOptions)).ToList())
                    services.Remove(option);

                services.AddDbContextPool<ImdbContext>(options =>
                {
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                    options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));


                    using var context = new ImdbContext(options.Options as DbContextOptions<ImdbContext>);

                    context.NameBasics.Add(GetNameBasics());
                    context.TitleAkas.Add(GetTitleAkas());
                    context.TitleBasics.Add(GetTitleBasics());
                    context.TitleCrew.Add(GetTitleCrew());
                    context.TitleEpisodes.Add(GetTitleEpisode());
                    context.TitlePrincipals.Add(GetTitlePrincipals());
                    context.TitleRatings.Add(GetTitleRating());

                    context.SaveChanges();
                });
            });
        });
    }

    [Fact]
    public async Task NameBasicsEndpointWorks() => await TestImdbEndpoint(GetNameBasics());

    [Fact]
    public async Task TitleAkasEndpointWorks() => await TestImdbEndpoint(GetTitleAkas());

    [Fact]
    public async Task TitleBasicsEndpointWorks() => await TestImdbEndpoint(GetTitleBasics());

    [Fact]
    public async Task TitleCrewEndpointWorks() => await TestImdbEndpoint(GetTitleCrew());

    [Fact]
    public async Task TitleEpisodeEndpointWorks() => await TestImdbEndpoint(GetTitleEpisode());

    [Fact]
    public async Task TitlePrincipalsEndpointWorks() => await TestImdbEndpoint(GetTitlePrincipals());

    [Fact]
    public async Task TitleRatingEndpointWorks() => await TestImdbEndpoint(GetTitleRating());

    private async Task TestImdbEndpoint<T>(T expectedResults) where T : class
        => await TestImdbEndpoint(new[] { expectedResults });

    private async Task TestImdbEndpoint<T>(T[] expectedResults) where T : class
    {
        var typeName = typeof(T).Name;

        var client = webApplicationFactory.CreateClient();

        var response = await client.GetAsync($"/ImDb/{typeName}");
        var odataResponse = await response.Content.ReadFromJsonAsync<ODataResponse<T>>();

        Assert.Equal($"http://localhost/ImDb/$metadata#{typeName}", odataResponse.Context);
        Assert.Equivalent(expectedResults, odataResponse.Results);
    }

    private class ODataResponse<T> where T : class
    {
        [JsonPropertyName("@odata.context")]
        public string Context { get; set; }

        [JsonPropertyName("value")]
        public T[] Results { get; set; }
    }

    private static NameBasics GetNameBasics() => new()
    {
        NameId = 1,
        Name = "Fred Astaire",
        BirthYear = 1899,
        DeathYear = 1987,
        Professions = "soundtrack,actor,miscellaneous",
        KnownForTitleIds = "50419,45537,72308,53137",
    };

    private static TitleAkas GetTitleAkas() => new()
    {
        TitleId = 1,
        Index = 1,
        Title = "Carmencita - spanyol tanc",
        Region = "HU",
        Language = "hu",
        Types = "imdbDisplay",
        Attributes = null,
        IsOriginalTitle = false,
    };

    private static TitleBasics GetTitleBasics() => new()
    {
        TitleId = 1,
        TitleType = "short",
        PrimaryTitle = "Carmencita",
        OriginalTitle = "Carmencita",
        IsAdult = false,
        StartYear = 1894,
        EndYear = 1895,
        RuntimeMinutes = 1,
        Genres = "Documentary,Short",
    };

    private static TitleCrew GetTitleCrew() => new()
    {
        TitleId = 1,
        DirectorNameIds = "5690",
        WriterNameIds = null,
    };

    private static TitleEpisode GetTitleEpisode() => new()
    {
        TitleId = 1,
        ParentTitleId = 41038,
        SeasonNumber = 1,
        EpisodeNumber = 9,
    };

    private static TitlePrincipals GetTitlePrincipals() => new()
    {
        TitleId = 1,
        Index = 1,
        NameId = 1588970,
        Category = "self",
        Job = null,
        Characters = "Self",
    };

    private static TitleRating GetTitleRating() => new()
    {
        TitleId = 1,
        AverageRating = 5.7,
        NumberOfVotes = 1974,
    };
}