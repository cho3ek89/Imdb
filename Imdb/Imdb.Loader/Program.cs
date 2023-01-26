﻿using Imdb.Common.DbContexts;
using Imdb.Common.DbContexts.Utilities;
using Imdb.Loader;
using Imdb.Loader.Options;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;

using System.Diagnostics;

var requiredService = GetServiceProvider().GetRequiredService<IImdbUpdateService>();
var cancellationSource = new CancellationTokenSource();

Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
{
    e.Cancel = true;
    cancellationSource.Cancel();
};

await requiredService.UpdateDatabase(cancellationSource.Token);

static IServiceProvider GetServiceProvider()
{
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    var serviceCollection = new ServiceCollection();

    serviceCollection.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();

        var logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
        loggingBuilder.AddSerilog(logger, true);
    });

    serviceCollection.AddOptions();

    serviceCollection.Configure<DatabaseSettings>(options =>
        config.GetSection("DatabaseSettings").Bind(options));

    serviceCollection.Configure<DownloadSettings>(options =>
        config.GetSection("DownloadSettings").Bind(options));

    serviceCollection.AddDbContext<ImdbContext>(options =>
    {
        options.UseSqlite(config.GetConnectionString("ImDb"));
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging(Debugger.IsAttached);
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        options.ReplaceService<IRelationalCommandBuilderFactory, SqliteRelationalCommandBuilderFactory>();
    }, ServiceLifetime.Singleton);

    serviceCollection.AddSingleton<IImdbFilesProvider, ImdbFilesProvider>();
    serviceCollection.AddSingleton<IImdbRepository, ImdbRepository>();
    serviceCollection.AddSingleton<IImdbUpdateService, ImdbUpdateService>();

    return serviceCollection.BuildServiceProvider();
}