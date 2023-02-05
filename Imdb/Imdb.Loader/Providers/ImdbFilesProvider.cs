using Imdb.Common.Helpers;
using Imdb.Loader.Models;
using Imdb.Loader.Services;

using Microsoft.Extensions.Options;

namespace Imdb.Loader.Providers
{
    public class ImdbFilesProvider : IImdbFilesProvider
    {
        private readonly DownloadSettings settings;

        private readonly ILogger<ImdbLoadingService> logger;

        public ImdbFilesProvider(IOptions<DownloadSettings> settings, ILogger<ImdbLoadingService> logger)
        {
            this.settings = settings.Value;
            this.logger = logger;
        }

        public async Task DeleteDownloadDirectory()
        {
            var downloadLocation = settings.DownloadLocation;

            try
            {
                await FileHelper.DeleteDirectory(downloadLocation);
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "{downloadLocation} directory cleanup failed.", downloadLocation);
            }
        }

        public async Task<ImdbFiles> DownloadAndDecompressFiles() =>
            await DownloadAndDecompressFiles(CancellationToken.None);

        public async Task<ImdbFiles> DownloadAndDecompressFiles(CancellationToken cancellationToken)
        {
            var sourceUrl = settings.SourceUrl;
            var downloadLocation = settings.DownloadLocation;
            var filesToDownload = new string[]
            {
                settings.FilesToDownload.NameBasics,
                settings.FilesToDownload.TitleAkas,
                settings.FilesToDownload.TitleBasics,
                settings.FilesToDownload.TitleCrew,
                settings.FilesToDownload.TitleEpisode,
                settings.FilesToDownload.TitlePrincipals,
                settings.FilesToDownload.TitleRatings
            };

            if (!Directory.Exists(downloadLocation))
                Directory.CreateDirectory(downloadLocation);

            logger?.LogInformation("Downloading and unpacking files started.");

            foreach (var fileToDownload in filesToDownload)
            {
                logger?.LogInformation("Downloading {fileToDownload}", fileToDownload);
                await FileHelper.DownloadFile(new Uri(sourceUrl, fileToDownload), Path.Combine(downloadLocation, fileToDownload), cancellationToken);

                logger?.LogInformation("Unpacking {fileToDownload}", fileToDownload);
                await FileHelper.DecompressGZipArchive(Path.Combine(downloadLocation, fileToDownload), cancellationToken);
            }

            logger?.LogInformation("Downloading and unpacking files completed.");

            var filesToLoad = GetFilesToLoad(settings.DownloadLocation, settings.FilesToDownload);
            return filesToLoad;
        }

        private static ImdbFiles GetFilesToLoad(string downloadLocation, ImdbFiles filesToDownload)
        {
            string GetFileToLoad(string archiveName)
            {
                var fileName = Path.GetFileNameWithoutExtension(archiveName);
                var fullFilePath = Path.Combine(downloadLocation, fileName);

                return fullFilePath;
            }

            return new()
            {
                NameBasics = GetFileToLoad(filesToDownload.NameBasics),
                TitleAkas = GetFileToLoad(filesToDownload.TitleAkas),
                TitleBasics = GetFileToLoad(filesToDownload.TitleBasics),
                TitleCrew = GetFileToLoad(filesToDownload.TitleCrew),
                TitleEpisode = GetFileToLoad(filesToDownload.TitleEpisode),
                TitlePrincipals = GetFileToLoad(filesToDownload.TitlePrincipals),
                TitleRatings = GetFileToLoad(filesToDownload.TitleRatings)
            };
        }
    }
}
