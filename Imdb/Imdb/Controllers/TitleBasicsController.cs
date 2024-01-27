using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class TitleBasicsController(ImdbContext context) : ImdbController<TitleBasics>(context)
{
}
