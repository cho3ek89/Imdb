using Imdb.Common.DbContexts;
using Imdb.Common.DbContexts.Utilities;
using Imdb.Loader.Models;
using Imdb.Loader.Providers;
using Imdb.Loader.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Serilog;

Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(options =>
    {
        options.AddJsonFile("appsettings.json");
    })
    .UseSerilog((context, options) =>
    {
        options.ReadFrom.Configuration(context.Configuration);
    })
    .ConfigureServices((host, services) =>
    {
        services.Configure<ConsoleLifetimeOptions>(options =>
            options.SuppressStatusMessages = true);

        services.Configure<DatabaseSettings>(options =>
            host.Configuration.GetSection("DatabaseSettings").Bind(options));

        services.Configure<DownloadSettings>(options =>
            host.Configuration.GetSection("DownloadSettings").Bind(options));

        services.AddDbContext<ImdbContext>(options =>
        {
            options.UseSqlite(host.Configuration.GetConnectionString("ImDb"));
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging(host.HostingEnvironment.IsDevelopment());
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            options.ReplaceService<IRelationalCommandBuilderFactory, SqliteRelationalCommandBuilderFactory>();
        }, ServiceLifetime.Singleton);

        services.AddSingleton<IImdbFilesProvider, ImdbFilesProvider>();
        services.AddSingleton<IImdbRepository, ImdbRepository>();
        services.AddSingleton<IImdbLoadingService, ImdbLoadingService>();

        services.AddHostedService<ImdbLoadingLauncher>();
    })
    .Build()
    .Run();