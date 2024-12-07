namespace RecipeManagement.Domain.Recipes.Models;

using Destructurama.Attributed;

public sealed record RecipeForUpdate
{
    public string Title { get; set; }

}
