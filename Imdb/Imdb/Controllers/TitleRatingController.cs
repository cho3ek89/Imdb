using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class TitleRatingController : ImdbController<TitleRating>
{
    public TitleRatingController(ImdbContext context) : base(context) { }
}
