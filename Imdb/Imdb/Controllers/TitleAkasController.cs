using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class TitleAkasController : ImdbController<TitleAkas>
{
    public TitleAkasController(ImdbContext context) : base(context) { }
}
