namespace Imdb.Loader;

using Imdb.Loader.Options;

using System.Threading;
using System.Threading.Tasks;

public interface IImdbRepository
{
    Task UpdateDatabase(ImdbFiles filesToLoad);

    Task UpdateDatabase(ImdbFiles filesToLoad, CancellationToken cancellationToken);
}
