namespace Imdb.Controllers
{
    using Microsoft.AspNet.OData.Query;
    using Microsoft.AspNetCore.Mvc;
    
    using System.Linq;

    using DbContexts;
    using Models;

    [ApiController]
    [Route("[controller]")]
    public class ImdbController : ControllerBase
    {
        private readonly ImdbContext context;

        public ImdbController(ImdbContext context)
        {
            this.context = context;
        }

        [HttpGet("name-basics")]
        public IActionResult GetNameBasics(ODataQueryOptions<NameBasics> options) =>
            GetResults(context.NameBasics, options);

        [HttpGet("title-akas")]
        public IActionResult GetTitleAkas(ODataQueryOptions<TitleAkas> options) =>
            GetResults(context.TitleAkas, options);

        [HttpGet("title-basics")]
        public IActionResult GetTitleBasics(ODataQueryOptions<TitleBasics> options) => 
            GetResults(context.TitleBasics, options);

        [HttpGet("title-crew")]
        public IActionResult GetTitleCrew(ODataQueryOptions<TitleCrew> options) =>
            GetResults(context.TitleCrew, options);

        [HttpGet("title-episodes")]
        public IActionResult GetTitleEpisodes(ODataQueryOptions<TitleEpisode> options) =>
            GetResults(context.TitleEpisodes, options);

        [HttpGet("title-principals")]
        public IActionResult GetTitlePrincipals(ODataQueryOptions<TitlePrincipals> options) =>
            GetResults(context.TitlePrincipals, options);

        [HttpGet("title-ratings")]
        public IActionResult GetTitleRatings(ODataQueryOptions<TitleRating> options) =>
            GetResults(context.TitleRatings, options);

        private IActionResult GetResults<T>(IQueryable entityQuery, ODataQueryOptions<T> options) where T: class
        {
            var resultQuery = options.ApplyTo(entityQuery) as IQueryable<dynamic>;

            var result = resultQuery.ToList();

            if (options.Count != null && options.Count.Value)
            {
                var resultCountQuery = options.ApplyTo(entityQuery,
                    AllowedQueryOptions.Skip | AllowedQueryOptions.Top) as IQueryable<dynamic>;

                var resultCount = resultCountQuery.Count();

                return Ok(new
                {
                    Result = result,
                    Count = resultCount,
                });
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
