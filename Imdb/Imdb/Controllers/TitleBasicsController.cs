using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class TitleBasicsController : ImdbController<TitleBasics>
{
    public TitleBasicsController(ImdbContext context) : base(context) { }
}
