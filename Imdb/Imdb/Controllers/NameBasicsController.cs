using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class NameBasicsController(ImdbContext context) : ImdbController<NameBasics>(context)
{
}
