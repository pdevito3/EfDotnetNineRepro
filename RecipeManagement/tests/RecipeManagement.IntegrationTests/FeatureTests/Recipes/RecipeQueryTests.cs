namespace RecipeManagement.IntegrationTests.FeatureTests.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class RecipeQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_recipe_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var recipeOne = new FakeRecipeBuilder().Build();
        await testingServiceScope.InsertAsync(recipeOne);

        // Act
        var query = new GetRecipe.Query(recipeOne.Id);
        var recipe = await testingServiceScope.SendAsync(query);

        // Assert
        recipe.Title.Should().Be(recipeOne.Title);

    }
}