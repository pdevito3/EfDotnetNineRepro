namespace RecipeManagement.IntegrationTests.FeatureTests.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteRecipeCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_recipe_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var recipe = new FakeRecipeBuilder().Build();
        await testingServiceScope.InsertAsync(recipe);

        // Act
        var command = new DeleteRecipe.Command(recipe.Id);
        await testingServiceScope.SendAsync(command);
        var recipeResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Recipes
                .CountAsync(r => r.Id == recipe.Id));

        // Assert
        recipeResponse.Should().Be(0);
    }
}