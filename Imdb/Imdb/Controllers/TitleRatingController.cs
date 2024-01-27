using Imdb.Common.DbContexts;
using Imdb.Common.Models;

namespace Imdb.Controllers;

public class TitleRatingController(ImdbContext context) : ImdbController<TitleRating>(context)
{
}
