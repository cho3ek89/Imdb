using Imdb.DbContexts;
using Imdb.Models;

using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddControllers(options =>
{
    options.EnableEndpointRouting = false;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
})
.AddOData(options =>
{
    options.AddRouteComponents("odata", GetEdmModel());

    options.Select();
    options.Filter();
    options.OrderBy();
    options.SkipToken();
    options.SetMaxTop(1000);
    options.Count();
});

builder.Services.AddDbContextPool<ImdbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("ImDb"));
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddMvc();

var app = builder.Build();

app.UseExceptionHandler("/exception");

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseCors();

app.UseMvc();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();

static IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();
    odataBuilder.EnableLowerCamelCase();

    odataBuilder.EntitySet<NameBasics>(nameof(NameBasics)).EntityType.HasKey(k => k.NameId);
    odataBuilder.EntitySet<TitleAkas>(nameof(TitleAkas)).EntityType.HasKey(k => new { k.TitleId, k.Index });
    odataBuilder.EntitySet<TitleBasics>(nameof(TitleBasics)).EntityType.HasKey(k => k.TitleId);
    odataBuilder.EntitySet<TitleEpisode>(nameof(TitleCrew)).EntityType.HasKey(k => k.TitleId);
    odataBuilder.EntitySet<TitleEpisode>(nameof(TitleEpisode)).EntityType.HasKey(k => k.TitleId);
    odataBuilder.EntitySet<TitleAkas>(nameof(TitlePrincipals)).EntityType.HasKey(k => new { k.TitleId, k.Index });
    odataBuilder.EntitySet<TitleEpisode>(nameof(TitleRating)).EntityType.HasKey(k => k.TitleId);

    return odataBuilder.GetEdmModel();
}