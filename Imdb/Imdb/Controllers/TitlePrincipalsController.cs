using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class TitlePrincipalsController : ImdbController<TitlePrincipals>
{
    public TitlePrincipalsController(ImdbContext context) : base(context) { }
}
