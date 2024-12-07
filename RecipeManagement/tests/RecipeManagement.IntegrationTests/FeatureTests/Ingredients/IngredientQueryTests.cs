namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class IngredientQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_ingredient_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var ingredientOne = new FakeIngredientBuilder().Build();
        await testingServiceScope.InsertAsync(ingredientOne);

        // Act
        var query = new GetIngredient.Query(ingredientOne.Id);
        var ingredient = await testingServiceScope.SendAsync(query);

        // Assert
        ingredient.Name.Should().Be(ingredientOne.Name);
        ingredient.Quantity.Should().Be(ingredientOne.Quantity);
        ingredient.ExpiresOn.Should().BeCloseTo((DateTime)ingredientOne.ExpiresOn, 1.Seconds());
        ingredient.BestTimeOfDay.Should().BeCloseTo((DateTimeOffset)ingredientOne.BestTimeOfDay, 1.Seconds());
        ingredient.Measure.Should().Be(ingredientOne.Measure);
    }

    [Fact]
    public async Task get_ingredient_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetIngredient.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}