namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Ingredients.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class AddListIngredientCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_ingredient_list_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var recipe = new FakeRecipeBuilder().Build();
        await testingServiceScope.InsertAsync(recipe);
        var ingredientOne = new FakeIngredientForCreationDto().Generate();
        var ingredientTwo = new FakeIngredientForCreationDto().Generate();

        // Act
        var command = new AddIngredientList.Command(new List<IngredientForCreationDto>() {ingredientOne, ingredientTwo}, recipe.Id);
        var ingredientReturned = await testingServiceScope.SendAsync(command);
        var firstReturned = ingredientReturned.FirstOrDefault();
        var secondReturned = ingredientReturned.Skip(1).FirstOrDefault();

        var ingredientDb = await testingServiceScope.ExecuteDbContextAsync(db => db.Ingredients
            .Where(x => x.Id == firstReturned.Id || x.Id == secondReturned.Id)
            .ToListAsync());
        var firstDbRecord = ingredientDb.FirstOrDefault(x => x.Id == firstReturned.Id);
        var secondDbRecord = ingredientDb.FirstOrDefault(x => x.Id == secondReturned.Id);

        // Assert
        firstReturned.Name.Should().Be(ingredientOne.Name);
        secondReturned.Name.Should().Be(ingredientTwo.Name);
        firstReturned.Quantity.Should().Be(ingredientOne.Quantity);
        secondReturned.Quantity.Should().Be(ingredientTwo.Quantity);
        firstReturned.ExpiresOn.Should().BeCloseTo((DateTime)ingredientOne.ExpiresOn, 1.Seconds());
        secondReturned.ExpiresOn.Should().BeCloseTo((DateTime)ingredientTwo.ExpiresOn, 1.Seconds());
        firstReturned.BestTimeOfDay.Should().BeCloseTo((DateTimeOffset)ingredientOne.BestTimeOfDay, 1.Seconds());
        secondReturned.BestTimeOfDay.Should().BeCloseTo((DateTimeOffset)ingredientTwo.BestTimeOfDay, 1.Seconds());
        firstReturned.Measure.Should().Be(ingredientOne.Measure);
        secondReturned.Measure.Should().Be(ingredientTwo.Measure);

        firstDbRecord.Name.Should().Be(ingredientOne.Name);
        secondDbRecord.Name.Should().Be(ingredientTwo.Name);
        firstDbRecord.Quantity.Should().Be(ingredientOne.Quantity);
        secondDbRecord.Quantity.Should().Be(ingredientTwo.Quantity);
        firstDbRecord.ExpiresOn.Should().BeCloseTo((DateTime)ingredientOne.ExpiresOn, 1.Seconds());
        secondDbRecord.ExpiresOn.Should().BeCloseTo((DateTime)ingredientTwo.ExpiresOn, 1.Seconds());
        firstDbRecord.BestTimeOfDay.Should().BeCloseTo((DateTimeOffset)ingredientOne.BestTimeOfDay, 1.Seconds());
        secondDbRecord.BestTimeOfDay.Should().BeCloseTo((DateTimeOffset)ingredientTwo.BestTimeOfDay, 1.Seconds());
        firstDbRecord.Measure.Should().Be(ingredientOne.Measure);
        secondDbRecord.Measure.Should().Be(ingredientTwo.Measure);

        firstDbRecord.Recipe.Id.Should().Be(recipe.Id);
        secondDbRecord.Recipe.Id.Should().Be(recipe.Id);
    }
}