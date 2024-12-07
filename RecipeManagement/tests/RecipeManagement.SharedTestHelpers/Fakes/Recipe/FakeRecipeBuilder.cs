namespace RecipeManagement.SharedTestHelpers.Fakes.Recipe;

using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.Models;

public class FakeRecipeBuilder
{
    private RecipeForCreation _creationData = new FakeRecipeForCreation().Generate();

    public FakeRecipeBuilder WithModel(RecipeForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeRecipeBuilder WithTitle(string title)
    {
        _creationData.Title = title;
        return this;
    }
    
    public Recipe Build()
    {
        var result = Recipe.Create(_creationData);
        return result;
    }
}