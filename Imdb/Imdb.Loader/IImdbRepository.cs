using Imdb.Loader.Options;

namespace Imdb.Loader;

public interface IImdbRepository
{
    Task UpdateDatabase(ImdbFiles filesToLoad);

    Task UpdateDatabase(ImdbFiles filesToLoad, CancellationToken cancellationToken);
}
