namespace Imdb
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.OData;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OData.Edm;
    using Microsoft.OData.ModelBuilder;

    using System.Text.Json;

    using DbContexts;
    using Models;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.EnableEndpointRouting = false;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.WriteIndented = Environment.IsDevelopment();
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

            services.AddDbContextPool<ImdbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("ImDb"));
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging(Environment.IsDevelopment());
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static IEdmModel GetEdmModel()
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
    }
}
