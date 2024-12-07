namespace RecipeManagement.IntegrationTests.FeatureTests.Recipes;

using RecipeManagement.Domain.Recipes.Features;
using RecipeManagement.Services;
using RecipeManagement.Databases;
using Bogus;
using Domain;
using System.Threading.Tasks;

public class PerformFakeBookMigrationTests : TestBase
{
    [Fact]
    public async Task can_perform_perform_fake_book_migration()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var user = Guid.NewGuid().ToString();
        var dbContextName = testingServiceScope.GetService<RecipesDbContext>();

        // Act
        var job = new PerformFakeBookMigration(dbContextName);
        var command = new PerformFakeBookMigration.Command() { User = user };
        await job.Handle(command, CancellationToken.None);

        // Assert
        // TODO job assertion
    }
}