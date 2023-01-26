using Imdb.Loader.Options;

namespace Imdb.Loader
{
    public interface IImdbFilesProvider
    {
        Task DeleteDownloadDirectory();

        Task<ImdbFiles> DownloadAndDecompressFiles();

        Task<ImdbFiles> DownloadAndDecompressFiles(CancellationToken cancellationToken);
    }
}
