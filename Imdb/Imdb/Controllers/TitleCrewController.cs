using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class TitleCrewController(ImdbContext context) : ImdbController<TitleCrew>(context)
{
}
