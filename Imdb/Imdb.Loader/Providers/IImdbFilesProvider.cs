using Imdb.Loader.Models;

namespace Imdb.Loader.Providers
{
    public interface IImdbFilesProvider
    {
        Task DeleteDownloadDirectory();

        Task<ImdbFiles> DownloadAndDecompressFiles();

        Task<ImdbFiles> DownloadAndDecompressFiles(CancellationToken cancellationToken);
    }
}
