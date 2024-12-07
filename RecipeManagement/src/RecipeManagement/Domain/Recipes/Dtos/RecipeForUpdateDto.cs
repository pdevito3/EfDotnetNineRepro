namespace RecipeManagement.Domain.Recipes.Dtos;

using Destructurama.Attributed;

public sealed record RecipeForUpdateDto
{
    public string Title { get; set; }

}
