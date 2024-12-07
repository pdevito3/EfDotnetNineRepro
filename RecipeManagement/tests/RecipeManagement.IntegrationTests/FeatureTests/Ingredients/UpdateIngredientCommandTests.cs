namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UpdateIngredientCommandTests : TestBase
{
    [Fact]
    public async Task can_update_existing_ingredient_in_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var ingredient = new FakeIngredientBuilder().Build();
        await testingServiceScope.InsertAsync(ingredient);
        var updatedIngredientDto = new FakeIngredientForUpdateDto().Generate();

        // Act
        var command = new UpdateIngredient.Command(ingredient.Id, updatedIngredientDto);
        await testingServiceScope.SendAsync(command);
        var updatedIngredient = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Ingredients
                .FirstOrDefaultAsync(i => i.Id == ingredient.Id));

        // Assert
        updatedIngredient.Name.Should().Be(updatedIngredientDto.Name);
        updatedIngredient.Quantity.Should().Be(updatedIngredientDto.Quantity);
        updatedIngredient.ExpiresOn.Should().BeCloseTo((DateTime)updatedIngredientDto.ExpiresOn, 1.Seconds());
        updatedIngredient.BestTimeOfDay.Should().BeCloseTo((DateTimeOffset)updatedIngredientDto.BestTimeOfDay, 1.Seconds());
        updatedIngredient.Measure.Should().Be(updatedIngredientDto.Measure);
        updatedIngredient.AverageCost.Amount.Should().BeApproximately(updatedIngredientDto.AverageCost, 0.005M);
    }
}