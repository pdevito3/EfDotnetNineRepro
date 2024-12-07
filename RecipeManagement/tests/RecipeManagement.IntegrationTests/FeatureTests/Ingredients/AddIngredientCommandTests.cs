namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RecipeManagement.Domain.Ingredients.Features;

public class AddIngredientCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_ingredient_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var ingredientOne = new FakeIngredientForCreationDto().Generate();

        // Act
        var command = new AddIngredient.Command(ingredientOne);
        var ingredientReturned = await testingServiceScope.SendAsync(command);
        var ingredientCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Ingredients
            .FirstOrDefaultAsync(i => i.Id == ingredientReturned.Id));

        // Assert
        ingredientReturned.Name.Should().Be(ingredientOne.Name);
        ingredientReturned.Quantity.Should().Be(ingredientOne.Quantity);
        ingredientReturned.ExpiresOn.Should().BeCloseTo((DateTime)ingredientOne.ExpiresOn, 1.Seconds());
        ingredientReturned.BestTimeOfDay.Should().BeCloseTo((DateTimeOffset)ingredientOne.BestTimeOfDay, 1.Seconds());
        ingredientReturned.Measure.Should().Be(ingredientOne.Measure);
        ingredientReturned.AverageCost.Should().BeApproximately(ingredientOne.AverageCost, 0.005M);

        ingredientCreated.Name.Should().Be(ingredientOne.Name);
        ingredientCreated.Quantity.Should().Be(ingredientOne.Quantity);
        ingredientCreated.ExpiresOn.Should().BeCloseTo((DateTime)ingredientOne.ExpiresOn, 1.Seconds());
        ingredientCreated.BestTimeOfDay.Should().BeCloseTo((DateTimeOffset)ingredientOne.BestTimeOfDay, 1.Seconds());
        ingredientCreated.Measure.Should().Be(ingredientOne.Measure);
        ingredientCreated.AverageCost.Amount.Should().BeApproximately(ingredientOne.AverageCost, 0.005M);
    }
}