namespace RecipeManagement.Extensions.Services;

using RecipeManagement.Databases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using HeimGuard;
using RecipeManagement.Resources;
using RecipeManagement.Services;
using RecipeManagement.Resources.HangfireUtilities;
using Resources;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
    {
        // DbContext -- Do Not Delete
        var connectionString = configuration.GetConnectionStringOptions().RecipeManagement;
        if(string.IsNullOrWhiteSpace(connectionString))
        {
            // this makes local migrations easier to manage. feel free to refactor if desired.
            connectionString = env.IsDevelopment() 
                ? "Host=localhost;Port=65298;Database=dev_recipemanagement;Username=postgres;Password=postgres"
                : throw new Exception("The database connection string is not set.");
        }

        services.AddDbContext<RecipesDbContext>(options =>
            options.UseNpgsql(connectionString,
                builder => builder.MigrationsAssembly(typeof(RecipesDbContext).Assembly.FullName))
                            .UseSnakeCaseNamingConvention());

        services.AddHostedService<MigrationHostedService<RecipesDbContext>>();

        services.SetupHangfire(env);

        // Auth -- Do Not Delete
        var authOptions = configuration.GetAuthOptions();
        if (!env.IsEnvironment(Consts.Testing.FunctionalTestingEnvName))
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authOptions.Authority;
                    options.Audience = authOptions.Audience;
                    options.RequireHttpsMetadata = !env.IsDevelopment();
                });
        }

        services.AddAuthorization(options =>
        {
        });
    }
}
    
public static class HangfireConfig
{
    public static void SetupHangfire(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddScoped<IJobContextAccessor, JobContextAccessor>();
        services.AddScoped<IJobWithUserContext, JobWithUserContext>();
        // if you want tags with sql server
        // var tagOptions = new TagsOptions() { TagsListStyle = TagsListStyle.Dropdown };
        
        // var hangfireConfig = new MemoryStorageOptions() { };
        services.AddHangfire(config =>
        {
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseMemoryStorage()
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                // if you want tags with sql server
                // .UseTagsWithSql(tagOptions, hangfireConfig)
                .UseActivator(new JobWithUserContextActivator(services.BuildServiceProvider()
                    .GetRequiredService<IServiceScopeFactory>()));
        });
        services.AddHangfireServer(options =>
        {
            options.WorkerCount = 10;
            options.ServerName = $"RecipeManagement-{env.EnvironmentName}";

            if (Consts.HangfireQueues.List().Length > 0)
            {
                options.Queues = Consts.HangfireQueues.List();
            }
        });

    }
}
