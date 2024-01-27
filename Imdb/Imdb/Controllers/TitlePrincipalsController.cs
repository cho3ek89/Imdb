using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class TitlePrincipalsController(ImdbContext context) : ImdbController<TitlePrincipals>(context)
{
}
