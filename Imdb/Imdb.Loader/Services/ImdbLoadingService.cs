using Imdb.Loader.Providers;

namespace Imdb.Loader.Services;

public class ImdbLoadingService(
    IImdbFilesProvider imdbFilesProvider,
    IImdbRepository imdbRepository,
    ILogger<ImdbLoadingService> logger) : IImdbLoadingService
{
    private readonly IImdbFilesProvider imdbFilesProvider = imdbFilesProvider;

    private readonly IImdbRepository imdbRepository = imdbRepository;

    private readonly ILogger<ImdbLoadingService> logger = logger;

    public async Task UpdateDatabase() =>
        await UpdateDatabase(CancellationToken.None);

    public async Task UpdateDatabase(CancellationToken cancellationToken)
    {
        try
        {
            var filesToLoad = await imdbFilesProvider.DownloadAndDecompressFiles(cancellationToken);

            await imdbRepository.UpdateDatabase(filesToLoad, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            logger?.LogWarning("An operation has been cancelled.");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "An error occured.");
        }
        finally
        {
            await imdbFilesProvider.DeleteDownloadDirectory();
        }
    }
}
