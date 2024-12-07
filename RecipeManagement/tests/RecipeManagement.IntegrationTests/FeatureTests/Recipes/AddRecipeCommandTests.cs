namespace RecipeManagement.IntegrationTests.FeatureTests.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RecipeManagement.Domain.Recipes.Features;

public class AddRecipeCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_recipe_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var recipeOne = new FakeRecipeForCreationDto().Generate();

        // Act
        var command = new AddRecipe.Command(recipeOne);
        var recipeReturned = await testingServiceScope.SendAsync(command);
        var recipeCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Recipes
            .FirstOrDefaultAsync(r => r.Id == recipeReturned.Id));

        // Assert
        recipeReturned.Title.Should().Be(recipeOne.Title);


        recipeCreated.Title.Should().Be(recipeOne.Title);

    }
}