using Imdb.Common.Models;

using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Imdb.Helpers;

public static class EdmModelBuilder
{
    public static IEdmModel GetImdbEdmModel()
    {
        var odataBuilder = new ODataConventionModelBuilder();

        odataBuilder.EntitySet<NameBasics>(nameof(NameBasics)).EntityType.HasKey(k => k.NameId);
        odataBuilder.EntitySet<TitleAkas>(nameof(TitleAkas)).EntityType.HasKey(k => new { k.TitleId, k.Index });
        odataBuilder.EntitySet<TitleBasics>(nameof(TitleBasics)).EntityType.HasKey(k => k.TitleId);
        odataBuilder.EntitySet<TitleCrew>(nameof(TitleCrew)).EntityType.HasKey(k => k.TitleId);
        odataBuilder.EntitySet<TitleEpisode>(nameof(TitleEpisode)).EntityType.HasKey(k => k.TitleId);
        odataBuilder.EntitySet<TitlePrincipals>(nameof(TitlePrincipals)).EntityType.HasKey(k => new { k.TitleId, k.Index });
        odataBuilder.EntitySet<TitleRating>(nameof(TitleRating)).EntityType.HasKey(k => k.TitleId);

        return odataBuilder.GetEdmModel();
    }
}
