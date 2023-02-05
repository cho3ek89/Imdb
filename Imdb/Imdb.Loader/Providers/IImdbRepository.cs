using Imdb.Loader.Models;

namespace Imdb.Loader.Providers;

public interface IImdbRepository
{
    Task UpdateDatabase(ImdbFiles filesToLoad);

    Task UpdateDatabase(ImdbFiles filesToLoad, CancellationToken cancellationToken);
}
