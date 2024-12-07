namespace RecipeManagement.Domain.Recipes.Models;

using Destructurama.Attributed;

public sealed record RecipeForCreation
{
    public string Title { get; set; }

}
