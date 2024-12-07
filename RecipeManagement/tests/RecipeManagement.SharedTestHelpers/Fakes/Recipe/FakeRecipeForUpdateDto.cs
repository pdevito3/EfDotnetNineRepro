namespace RecipeManagement.SharedTestHelpers.Fakes.Recipe;

using AutoBogus;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Domain.RecipeVisibilities;

public sealed class FakeRecipeForUpdateDto : AutoFaker<RecipeForUpdateDto>
{
    public FakeRecipeForUpdateDto()
    {
        RuleFor(r => r.Visibility, f => f.PickRandom(RecipeVisibility.ListNames()));
    }
}