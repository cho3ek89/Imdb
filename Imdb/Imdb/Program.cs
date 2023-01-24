using Imdb.Common.DbContexts;
using Imdb.Helpers;

using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddControllers()
.AddOData(options =>
{
    options.AddRouteComponents("ImDb", EdmModelBuilder.GetImdbEdmModel());
    options.EnableQueryFeatures(10000);
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

var app = builder.Build();

app.UseExceptionHandler("/exception");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors();

app.MapControllers();

app.Run();