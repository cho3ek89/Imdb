using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class TitleCrewController : ImdbController<TitleCrew>
{
    public TitleCrewController(ImdbContext context) : base(context) { }
}
