using System.IO.Compression;

namespace Imdb.Loader.Helpers;

public static class FileHelper
{
    private static readonly HttpClient httpClient;

    private const int BufferSize = 4096;

    static FileHelper()
    {
        var socketsHttpHandler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2)
        };

        httpClient = new HttpClient(socketsHttpHandler);
    }

    public static async Task DownloadFile(Uri fileToDownload, string fileDownloaded) =>
        await DownloadFile(fileToDownload, fileDownloaded, CancellationToken.None);

    public static async Task DownloadFile(Uri fileToDownload, string fileDownloaded, CancellationToken cancellationToken)
    {
        if (!Uri.IsWellFormedUriString(fileToDownload.ToString(), UriKind.Absolute))
            throw new ArgumentException("Invalid file url has been provided!");

        using var responseStream = await httpClient.GetStreamAsync(fileToDownload, cancellationToken);
        using var writeStream = new FileStream(fileDownloaded, FileMode.CreateNew);

        await responseStream.CopyToAsync(writeStream, BufferSize, cancellationToken);
    }

    public static async Task DecompressGZipArchive(string fileToDecompress) => 
        await DecompressGZipArchive(fileToDecompress, CancellationToken.None);

    public static async Task DecompressGZipArchive(string fileToDecompress, CancellationToken cancellationToken)
    {
        using var readStream = new FileStream(fileToDecompress, FileMode.Open);
        var decompressedFile = Path.Combine(Path.GetDirectoryName(fileToDecompress), Path.GetFileNameWithoutExtension(fileToDecompress));
        using var decompressionStream = new GZipStream(readStream, CompressionMode.Decompress);
        using var writeStream = new FileStream(decompressedFile, FileMode.CreateNew);

        await decompressionStream.CopyToAsync(writeStream, BufferSize, cancellationToken);
    }

    public static async Task DeleteDirectory(string directoryPath, int numberOfAttempts = 15, int delayMiliseconds = 30)
    {
        while (true)
        {
            try
            {
                Directory.Delete(directoryPath, true);
                return;
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception)
            {
                numberOfAttempts--;

                if (numberOfAttempts == 0) throw;

                await Task.Delay(delayMiliseconds);
            }
        }
    }
}
