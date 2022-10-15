using Imdb.Common.DbContexts;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Imdb.Controllers;

public abstract class ImdbController<T> : ODataController where T : class
{
    protected readonly ImdbContext context;

    public ImdbController(ImdbContext context)
    {
        this.context = context;
    }

    [EnableQuery]
    public IQueryable<T> Get() => context.Set<T>().AsQueryable();
}