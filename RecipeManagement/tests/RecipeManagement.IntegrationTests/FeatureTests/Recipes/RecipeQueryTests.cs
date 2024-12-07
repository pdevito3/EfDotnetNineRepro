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
        recipe.Directions.Should().Be(recipeOne.Directions);
        recipe.DateOfOrigin.Should().Be(recipeOne.DateOfOrigin);
        recipe.HaveMadeItMyself.Should().Be(recipeOne.HaveMadeItMyself);
        recipe.Tags.Should().BeEquivalentTo(recipeOne.Tags);
        recipe.Visibility.Should().Be(recipeOne.Visibility.Value);
        recipe.Rating.Should().Be(recipeOne.Rating.Value);
    }

    [Fact]
    public async Task get_recipe_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetRecipe.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task must_be_permitted()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        testingServiceScope.SetUserNotPermitted(Permissions.CanReadRecipes);

        // Act
        var command = new GetRecipe.Query(Guid.NewGuid());
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}