namespace Imdb.Loader;

using System.Threading;
using System.Threading.Tasks;

public interface IImdbUpdateService
{
    Task UpdateDatabase();

    Task UpdateDatabase(CancellationToken cancellationToken);
}
