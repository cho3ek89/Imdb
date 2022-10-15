using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class TitleEpisodeController : ImdbController<TitleEpisode>
{
    public TitleEpisodeController(ImdbContext context) : base(context) { }
}
