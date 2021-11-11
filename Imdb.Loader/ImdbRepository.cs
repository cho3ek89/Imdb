namespace Imdb.Loader;

using CsvHelper;
using CsvHelper.Configuration;

using Imdb.Models;
using Imdb.Loader.Helpers;
using Imdb.Loader.Options;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class ImdbRepository : IImdbRepository
{
    private readonly DatabaseSettings settings;

    private readonly ILogger<ImdbRepository> logger;

    public ImdbRepository(IOptions<DatabaseSettings> settings, ILogger<ImdbRepository> logger)
    {
        this.settings = settings.Value;
        this.logger = logger;
    }

    public async Task UpdateDatabase(ImdbFiles filesToLoad)
    {
        await UpdateDatabase(filesToLoad, CancellationToken.None);
    }

    public async Task UpdateDatabase(ImdbFiles filesToLoad, CancellationToken cancellationToken)
    {
        using var connection = new SqliteConnection(settings.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await CreateTablesIfMissing(connection, cancellationToken);

        logger?.LogInformation("Updating database started.");

        using (var command = new SqliteCommand(SqlQueries.PragmaStatements, connection))
            await command.ExecuteNonQueryAsync(cancellationToken);

        using var transaction = connection.BeginTransaction();

        try
        {
            await UpdateTable<NameBasics>(connection, transaction, filesToLoad.NameBasics, cancellationToken);
            await UpdateTable<TitleAkas>(connection, transaction, filesToLoad.TitleAkas, cancellationToken);
            await UpdateTable<TitleBasics>(connection, transaction, filesToLoad.TitleBasics, cancellationToken);
            await UpdateTable<TitleCrew>(connection, transaction, filesToLoad.TitleCrew, cancellationToken);
            await UpdateTable<TitleEpisode>(connection, transaction, filesToLoad.TitleEpisode, cancellationToken);
            await UpdateTable<TitlePrincipals>(connection, transaction, filesToLoad.TitlePrincipals, cancellationToken);
            await UpdateTable<TitleRating>(connection, transaction, filesToLoad.TitleRatings, cancellationToken);

            logger?.LogDebug("Committing transaction.");
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            logger?.LogDebug("Rolling back transaction.");
            transaction.Rollback();
            throw;
        }

        if (settings.VacuumDatabase)
            await VacuumDatabase(connection, cancellationToken);

        logger.LogInformation("Updating database completed.");
    }

    private async Task CreateTablesIfMissing(SqliteConnection connection, CancellationToken cancellationToken)
    {
        logger?.LogDebug(@"Creating missing database tables started.");

        using var command = new SqliteCommand(SqlQueries.CreateMissingImdbTables, connection);
        await command.ExecuteNonQueryAsync(cancellationToken);

        logger?.LogDebug(@"Creating missing database tables completed.");
    }

    private async Task UpdateTable<T>(SqliteConnection connection, SqliteTransaction transaction, string fileToLoad, CancellationToken cancellationToken) where T : class, new()
    {
        string tableName = GetTableName<T>();
        string sqlQuery = string.Format("INSERT INTO [{0}] VALUES ({1})", tableName, string.Join(",", from s in typeof(T).GetProperties()
                                                                                                      select "@" + s.Name));
        logger.LogInformation("Updating {tableName} table started.", tableName);

        using (var command = new SqliteCommand())
        {
            command.Connection = connection;
            command.Transaction = transaction;
            command.CommandText = "DELETE FROM [" + tableName + "]";
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        int count = 0;

        using (var command = new SqliteCommand(sqlQuery, connection, transaction))
        using (var csvReader = GetCsvReader(fileToLoad))
        {
            while (await csvReader.ReadAsync())
            {
                var record = csvReader.GetRecord<T>();

                command.Parameters.Clear();
                command.Parameters.AddRange(from s in typeof(T).GetProperties()
                                            select new SqliteParameter(s.Name, s.GetValue(record) ?? DBNull.Value));

                await command.ExecuteNonQueryAsync(cancellationToken);

                int num = count + 1;
                count = num;

                if (num % 100000 == 0)
                    logger.LogDebug("Saving changes ({count} entires processed)", count);
            }
        }

        logger.LogInformation("Updating {tableName} table completed.", tableName);
    }

    private async Task VacuumDatabase(SqliteConnection connection, CancellationToken cancellationToken)
    {
        const string vacuumCommandText = "VACUUM";

        logger?.LogInformation(@"Executing {vacuumCommandText} command started.", vacuumCommandText);

        using var command = new SqliteCommand(SqlQueries.Vacuum, connection);
        await command.ExecuteNonQueryAsync(cancellationToken);

        logger?.LogInformation(@"Executing {vacuumCommandText} command completed.", vacuumCommandText);
    }

    private static string GetTableName<T>()
    {
        return typeof(T).Name switch
        {
            nameof(NameBasics) => "name.basics",
            nameof(TitleAkas) => "title.akas",
            nameof(TitleBasics) => "title.basics",
            nameof(TitleCrew) => "title.crew",
            nameof(TitleEpisode) => "title.episodes",
            nameof(TitlePrincipals) => "title.principals",
            nameof(TitleRating) => "title.ratings",
            _ => throw new NotSupportedException("There is no database table for given model."),
        };
    }

    private CsvReader GetCsvReader(string fileNamePath)
    {
        var csvReaderConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            BadDataFound = new BadDataFound(a =>
                logger.LogWarning("Corrupted row has been found: {NewLine}{RawRecord}", Environment.NewLine, a.RawRecord)),

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
