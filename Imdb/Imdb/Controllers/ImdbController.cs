using Imdb.Common.DbContexts;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Imdb.Controllers;

public abstract class ImdbController<T>(ImdbContext context) : ODataController where T : class
{
    protected readonly ImdbContext context = context;

    [EnableQuery]
    public virtual IQueryable<T> Get() => context.Set<T>().AsQueryable();
}