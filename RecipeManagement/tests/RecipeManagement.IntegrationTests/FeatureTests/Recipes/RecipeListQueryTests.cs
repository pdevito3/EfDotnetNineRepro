namespace RecipeManagement.IntegrationTests.FeatureTests.Recipes;

using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes.Features;
using Domain;
using System.Threading.Tasks;

public class RecipeListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_recipe_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var recipeOne = new FakeRecipeBuilder().Build();
        var recipeTwo = new FakeRecipeBuilder().Build();
        var queryParameters = new RecipeParametersDto();

        await testingServiceScope.InsertAsync(recipeOne, recipeTwo);

        // Act
        var query = new GetRecipeList.Query(queryParameters);
        var recipes = await testingServiceScope.SendAsync(query);

        // Assert
        recipes.Count.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task must_be_permitted()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        testingServiceScope.SetUserNotPermitted(Permissions.CanReadRecipes);
        var queryParameters = new RecipeParametersDto();

        // Act
        var command = new GetRecipeList.Query(queryParameters);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}