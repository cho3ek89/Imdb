using Microsoft.Extensions.Logging;

namespace Imdb.Loader;

public class ImdbUpdateService : IImdbUpdateService
{
    private readonly IImdbFilesProvider imdbFilesProvider;

    private readonly IImdbRepository imdbRepository;

    private readonly ILogger<ImdbUpdateService> logger;

    public ImdbUpdateService(IImdbFilesProvider imdbFilesProvider, IImdbRepository imdbRepository, ILogger<ImdbUpdateService> logger)
    {
        this.imdbFilesProvider = imdbFilesProvider;
        this.imdbRepository = imdbRepository;
        this.logger = logger;
    }

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
