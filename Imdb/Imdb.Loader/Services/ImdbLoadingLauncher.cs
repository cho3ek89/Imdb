namespace Imdb.Loader.Services;

public class ImdbLoadingLauncher : BackgroundService
{
    private readonly IHostApplicationLifetime hostAppLifetime;

    private readonly IImdbLoadingService loadingService;

    public ImdbLoadingLauncher(IHostApplicationLifetime hostAppLifetime, IImdbLoadingService loadingService)
    {
        this.hostAppLifetime = hostAppLifetime;
        this.loadingService = loadingService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await loadingService.UpdateDatabase(stoppingToken);

        hostAppLifetime.StopApplication();
    }
}
