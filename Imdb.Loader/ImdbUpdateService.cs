namespace Imdb.Loader
{
	using System;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;
    
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;

	using Helpers;
	using Options;

	public class ImdbUpdateService : IImdbUpdateService
	{
		private readonly IImdbRepository iDbRepository;

		private readonly DownloadSettings settings;

		private readonly ILogger<ImdbUpdateService> logger;

		public ImdbUpdateService(IImdbRepository iDbRepository, IOptions<DownloadSettings> settings, ILogger<ImdbUpdateService> logger)
		{
			this.settings = settings.Value;
			this.iDbRepository = iDbRepository;
			this.logger = logger;
		}

		public async Task UpdateDatabase()
		{
			await UpdateDatabase(CancellationToken.None);
		}

		public async Task UpdateDatabase(CancellationToken cancellationToken)
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

			try
			{
				if (!Directory.Exists(downloadLocation))
					Directory.CreateDirectory(downloadLocation);

				foreach (var fileToDownload in filesToDownload)
				{
					logger.LogInformation("Downloading {0}", fileToDownload);
					await FileHelper.DownloadFile(new Uri(sourceUrl, fileToDownload), Path.Combine(downloadLocation, fileToDownload), cancellationToken);

					logger.LogInformation("Unpacking {0}", fileToDownload);
					await FileHelper.DecompressGZipArchive(Path.Combine(downloadLocation, fileToDownload), cancellationToken);
				}

				var filesToLoad = GetFilesToLoad(settings.DownloadLocation, settings.FilesToDownload);
				await iDbRepository.UpdateDatabase(filesToLoad, cancellationToken);
			}
			catch (OperationCanceledException)
            {
				logger.LogWarning("An operation has been cancelled.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error occured.");
			}
			finally
            {
				try
				{
					await FileHelper.DeleteDirectory(downloadLocation);
				}
				catch (Exception ex)
				{
					logger.LogWarning(ex, "{0} directory cleanup failed.", downloadLocation);
				}
			}
		}

		private static ImdbFiles GetFilesToLoad(string downloadLocation, ImdbFiles filesToDownload)
        {
			string GetFileToLoad(string archiveName)
			{
				var fileName = Path.GetFileNameWithoutExtension(archiveName);
				var fullFilePath = Path.Combine(downloadLocation, fileName);

				return fullFilePath;
			}

			return new ImdbFiles
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
