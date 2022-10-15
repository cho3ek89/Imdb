using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class NameBasicsController : ImdbController<NameBasics>
{
    public NameBasicsController(ImdbContext context) : base(context) { }
}
