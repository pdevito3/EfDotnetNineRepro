namespace RecipeManagement.Domain.Recipes.Dtos;

using Destructurama.Attributed;

public sealed record RecipeForCreationDto
{
    public string Title { get; set; }

}
