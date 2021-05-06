namespace Imdb
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNet.OData.Builder;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OData.Edm;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

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
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                options.SerializerSettings.Formatting = Environment.IsDevelopment() ? Formatting.Indented : Formatting.None;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
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

            services.AddOData();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Filter().OrderBy().MaxTop(null).SkipToken().Count();
                routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
                routeBuilder.EnableDependencyInjection();
            });

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
