namespace RecipeManagement.SharedTestHelpers.Fakes.Ingredient;

using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Models;

public class FakeIngredientBuilder
{
    private IngredientForCreation _creationData = new FakeIngredientForCreation().Generate();

    public FakeIngredientBuilder WithModel(IngredientForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeIngredientBuilder WithName(string name)
    {
        _creationData.Name = name;
        return this;
    }
    
    public FakeIngredientBuilder WithQuantity(string quantity)
    {
        _creationData.Quantity = quantity;
        return this;
    }
    
    public FakeIngredientBuilder WithExpiresOn(DateTime? expiresOn)
    {
        _creationData.ExpiresOn = expiresOn;
        return this;
    }
    
    public FakeIngredientBuilder WithBestTimeOfDay(DateTimeOffset? bestTimeOfDay)
    {
        _creationData.BestTimeOfDay = bestTimeOfDay;
        return this;
    }
    
    public FakeIngredientBuilder WithMeasure(string measure)
    {
        _creationData.Measure = measure;
        return this;
    }
    
    public Ingredient Build()
    {
        var result = Ingredient.Create(_creationData);
        return result;
    }
}