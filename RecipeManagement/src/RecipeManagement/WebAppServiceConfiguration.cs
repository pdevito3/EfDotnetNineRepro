namespace RecipeManagement;

using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RecipeManagement.Resources;
using Serilog;

public static class WebAppServiceConfiguration
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RecipeManagementOptions>(builder.Configuration.GetSection(RecipeManagementOptions.SectionName));

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddSingleton(Log.Logger);

        // TODO update CORS for your env
        var connectionString = builder.Configuration.GetConnectionStringOptions().RecipeManagement;
        if(string.IsNullOrWhiteSpace(connectionString))
        {
            // this makes local migrations easier to manage. feel free to refactor if desired.
            connectionString = builder.Environment.IsDevelopment() 
                ? "Host=localhost;Port=65298;Database=dev_recipemanagement;Username=postgres;Password=postgres"
                : throw new Exception("The database connection string is not set.");
        }

        builder.Services.AddDbContext<RecipesDbContext>(options =>
            options.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly(typeof(RecipesDbContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention());

        builder.Services.AddHostedService<MigrationHostedService<RecipesDbContext>>();

        builder.Services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        builder.Services.AddMvc();
        
        builder.Services.AddSwaggerGen(config =>
        {
            config.CustomSchemaIds(type => type.ToString().Replace("+", "."));
            config.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date"
            });

            config.IncludeXmlComments(string.Format(@$"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}RecipeManagement.WebApi.xml"));
        });
    }
}
