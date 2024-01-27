using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class TitleAkasController(ImdbContext context) : ImdbController<TitleAkas>(context)
{
}
