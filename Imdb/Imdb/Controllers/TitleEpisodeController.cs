using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class TitleEpisodeController(ImdbContext context) : ImdbController<TitleEpisode>(context)
{
}
