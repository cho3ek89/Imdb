namespace Imdb.Loader
{
    using System.Threading;
    using System.Threading.Tasks;

	using Options;
	public interface IImdbRepository
	{
		Task UpdateDatabase(ImdbFiles filesToLoad);

		Task UpdateDatabase(ImdbFiles filesToLoad, CancellationToken cancellationToken);
	}
}
