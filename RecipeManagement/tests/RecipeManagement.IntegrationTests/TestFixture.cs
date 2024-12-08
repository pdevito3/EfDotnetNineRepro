namespace RecipeManagement.IntegrationTests;

using NSubstitute;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using static Resources.RecipeManagementOptions;

[CollectionDefinition(nameof(TestFixture))]
public class TestFixtureCollection : ICollectionFixture<TestFixture> {}

public class TestFixture : IAsyncLifetime
{
    public static IServiceScopeFactory BaseScopeFactory;
    private PostgreSqlContainer _dbContainer;

    public async Task InitializeAsync()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "LocalIntegrationTesting"
        });

        _dbContainer = new PostgreSqlBuilder().Build();
        await _dbContainer.StartAsync();
        builder.Configuration.GetSection(ConnectionStringOptions.SectionName)[ConnectionStringOptions.RecipeManagementKey] = _dbContainer.GetConnectionString();
        await RunMigration(_dbContainer.GetConnectionString());
        
        builder.ConfigureServices();
        var services = builder.Services;

        var provider = services.BuildServiceProvider();
        BaseScopeFactory = provider.GetService<IServiceScopeFactory>();
    }

    private static async Task RunMigration(string connectionString)
    {
        var options = new DbContextOptionsBuilder<RecipesDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        var context = new RecipesDbContext(options);
        await context?.Database?.MigrateAsync();
    }

    public async Task DisposeAsync()
    {        
        await _dbContainer.DisposeAsync();
    }
}