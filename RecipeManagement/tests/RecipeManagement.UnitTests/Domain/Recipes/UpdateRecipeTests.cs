namespace RecipeManagement.UnitTests.Domain.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class UpdateRecipeTests
{
    private readonly Faker _faker;

    public UpdateRecipeTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_recipe()
    {
        // Arrange
        var recipe = new FakeRecipeBuilder().Build();
        var updatedRecipe = new FakeRecipeForUpdate().Generate();
        
        // Act
        recipe.Update(updatedRecipe);

        // Assert
        recipe.Title.Should().Be(updatedRecipe.Title);
        recipe.Directions.Should().Be(updatedRecipe.Directions);
        recipe.DateOfOrigin.Should().Be(updatedRecipe.DateOfOrigin);
        recipe.HaveMadeItMyself.Should().Be(updatedRecipe.HaveMadeItMyself);
        recipe.Tags.Should().BeEquivalentTo(updatedRecipe.Tags);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var recipe = new FakeRecipeBuilder().Build();
        var updatedRecipe = new FakeRecipeForUpdate().Generate();
        recipe.DomainEvents.Clear();
        
        // Act
        recipe.Update(updatedRecipe);

        // Assert
        recipe.DomainEvents.Count.Should().Be(1);
        recipe.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(RecipeUpdated));
    }
}