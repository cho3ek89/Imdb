using CsvHelper;
using CsvHelper.Configuration;

using EFCore.BulkExtensions;

using Imdb.Common.DbContexts;
using Imdb.Common.Models;
using Imdb.Loader.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using System.Globalization;

namespace Imdb.Loader.Providers;

public class ImdbRepository(
    ImdbContext context,
    IOptions<DatabaseSettings> settings,
    ILogger<ImdbRepository> logger) : IImdbRepository
{
    private readonly ImdbContext context = context;

    private readonly DatabaseSettings settings = settings.Value;

    private readonly ILogger<ImdbRepository> logger = logger;

    public async Task UpdateDatabase(ImdbFiles filesToLoad) =>
        await UpdateDatabase(filesToLoad, CancellationToken.None);

    public async Task UpdateDatabase(ImdbFiles filesToLoad, CancellationToken cancellationToken)
    {
        logger?.LogInformation("Creating database if missing.");
        await context.Database.EnsureCreatedAsync(cancellationToken);

        logger?.LogInformation("Updating database started.");

        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await UpdateTable<NameBasics>(context, filesToLoad.NameBasics, cancellationToken);
            await UpdateTable<TitleAkas>(context, filesToLoad.TitleAkas, cancellationToken);
            await UpdateTable<TitleBasics>(context, filesToLoad.TitleBasics, cancellationToken);
            await UpdateTable<TitleCrew>(context, filesToLoad.TitleCrew, cancellationToken);
            await UpdateTable<TitleEpisode>(context, filesToLoad.TitleEpisode, cancellationToken);
            await UpdateTable<TitlePrincipals>(context, filesToLoad.TitlePrincipals, cancellationToken);
            await UpdateTable<TitleRating>(context, filesToLoad.TitleRatings, cancellationToken);

            logger?.LogInformation("Committing transaction.");
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            logger?.LogInformation("Rolling back transaction.");
            transaction.Rollback();
            throw;
        }

        if (settings.VacuumDatabase)
        {
            const string vacuumCommandText = "VACUUM";
            logger?.LogInformation(@"Executing {vacuumCommandText} command.", vacuumCommandText);
            await context.Database.ExecuteSqlRawAsync(vacuumCommandText, cancellationToken);
        }

        logger?.LogInformation("Updating database completed.");
    }

    private async Task UpdateTable<T>(ImdbContext context, string fileToLoad, CancellationToken cancellationToken) where T : class, new()
    {
        var tableName = context.Model.FindEntityType(typeof(T)).GetSchemaQualifiedTableName();

        logger?.LogInformation("Updating {tableName} table started.", tableName);

        await context.Set<T>().ExecuteDeleteAsync(cancellationToken);

        var records = new List<T>();
        int recordsCount = 0;
        int recordsTotalCount = 0;
        var bulkConfig = new BulkConfig { BatchSize = settings.BatchSize };

        async Task BulkInsertRecordsAsync()
        {
            await context.BulkInsertAsync(
                entities: records,
                bulkConfig: bulkConfig,
                cancellationToken: cancellationToken);

            recordsTotalCount += recordsCount;
            recordsCount = 0;
            records.Clear();

            logger?.LogDebug("Saving changes ({recordsTotalCount} entires processed).", recordsTotalCount);
        }

        using var csvReader = GetCsvReader(fileToLoad);

        while (await csvReader.ReadAsync())
        {
            var record = csvReader.GetRecord<T>();
            records.Add(record);

            if (++recordsCount % settings.BatchSize == 0)
                await BulkInsertRecordsAsync();
        }

        await BulkInsertRecordsAsync();

        logger?.LogInformation("Updating {tableName} table completed.", tableName);
    }

    private CsvReader GetCsvReader(string fileNamePath)
    {
        var csvReaderConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            BadDataFound = new BadDataFound(a =>
                logger?.LogWarning("Corrupted row has been found: {NewLine}{RawRecord}", Environment.NewLine, a.RawRecord)),

            Delimiter = "\t",
            HasHeaderRecord = true,
            Mode = CsvMode.NoEscape,
        };

        var csvReader = new CsvReader(File.OpenText(fileNamePath), csvReaderConfig);

        csvReader.Context.RegisterClassMap<ImdbFilesCsvMap.NameBasicsMap>();
        csvReader.Context.RegisterClassMap<ImdbFilesCsvMap.TitleAkasMap>();
        csvReader.Context.RegisterClassMap<ImdbFilesCsvMap.TitleBasicsMap>();
        csvReader.Context.RegisterClassMap<ImdbFilesCsvMap.TitleCrewMap>();
        csvReader.Context.RegisterClassMap<ImdbFilesCsvMap.TitleEpisodeMap>();
        csvReader.Context.RegisterClassMap<ImdbFilesCsvMap.TitlePrincipalsMap>();
        csvReader.Context.RegisterClassMap<ImdbFilesCsvMap.TitleRatingMap>();

        return csvReader;
    }
}
