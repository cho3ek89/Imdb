using System.IO.Compression;

namespace Imdb.Loader.Helpers;

public static class FileHelper
{
    public static async Task DownloadFile(Uri fileToDownload, string fileDownloaded) =>
        await DownloadFile(fileToDownload, fileDownloaded, CancellationToken.None);

    public static async Task DownloadFile(Uri fileToDownload, string fileDownloaded, CancellationToken cancellationToken)
    {
        if (!Uri.IsWellFormedUriString(fileToDownload.ToString(), UriKind.Absolute))
            throw new ArgumentException("Invalid file url has been provided!");

        using var client = new HttpClient();
        using var responseStream = await client.GetStreamAsync(fileToDownload, cancellationToken);
        using var fileStream = new FileStream(fileDownloaded, FileMode.CreateNew);

        var bufferSize = 4096;
        var buffer = new byte[bufferSize];

        var bytesRead = responseStream.Read(buffer, 0, bufferSize);

        while (bytesRead > 0)
        {
            await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
            bytesRead = await responseStream.ReadAsync(buffer.AsMemory(0, bufferSize), cancellationToken);
        }
    }

    public static async Task DecompressGZipArchive(string fileToDecompress) => 
        await DecompressGZipArchive(fileToDecompress, CancellationToken.None);

    public static async Task DecompressGZipArchive(string fileToDecompress, CancellationToken cancellationToken)
    {
        using var readStream = new FileStream(fileToDecompress, FileMode.Open);
        var decompressedFile = Path.Combine(Path.GetDirectoryName(fileToDecompress), Path.GetFileNameWithoutExtension(fileToDecompress));
        using var writeStream = new FileStream(decompressedFile, FileMode.CreateNew);
        using var decompressionStream = new GZipStream(readStream, CompressionMode.Decompress);

        var bufferSize = 4096;
        var buffer = new byte[bufferSize];

        var bytesRead = decompressionStream.Read(buffer, 0, bufferSize);

        while (bytesRead > 0)
        {
            await writeStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
            bytesRead = await decompressionStream.ReadAsync(buffer.AsMemory(0, bufferSize), cancellationToken);
        }
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
