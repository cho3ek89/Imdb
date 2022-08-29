namespace Imdb.Loader;

public interface IImdbUpdateService
{
    Task UpdateDatabase();

    Task UpdateDatabase(CancellationToken cancellationToken);
}
