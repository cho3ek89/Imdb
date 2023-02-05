namespace Imdb.Loader.Services;

public interface IImdbLoadingService
{
    Task UpdateDatabase();

    Task UpdateDatabase(CancellationToken cancellationToken);
}
