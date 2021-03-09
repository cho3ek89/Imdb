namespace Imdb.Loader
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Serilog;

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Options;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var requiredService = GetServiceProvider().GetRequiredService<IImdbUpdateService>();
            var cancellationSource = new CancellationTokenSource();

            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                cancellationSource.Cancel();
            };

            await requiredService.UpdateDatabase(cancellationSource.Token);
        }

        private static IServiceProvider GetServiceProvider()
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

            serviceCollection.AddSingleton<IImdbRepository, ImdbRepository>();
            serviceCollection.AddSingleton<IImdbUpdateService, ImdbUpdateService>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
