namespace Imdb.Loader.Services;

public class ImdbLoadingLauncher(
    IHostApplicationLifetime hostAppLifetime, 
    IImdbLoadingService loadingService) : BackgroundService
{
    private readonly IHostApplicationLifetime hostAppLifetime = hostAppLifetime;

    private readonly IImdbLoadingService loadingService = loadingService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await loadingService.UpdateDatabase(stoppingToken);

        hostAppLifetime.StopApplication();
    }
}
