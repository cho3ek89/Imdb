using Microsoft.Extensions.Hosting;

namespace Imdb.Loader.Services
{
    public class ImdbLoadingLauncher : BackgroundService
    {
        private readonly IImdbLoadingService loadingService;

        public ImdbLoadingLauncher(IImdbLoadingService loadingService)
        {
            this.loadingService = loadingService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) =>
            await loadingService.UpdateDatabase(stoppingToken);
    }
}
