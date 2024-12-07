namespace RecipeManagement.Domain.Recipes.Dtos;

using Destructurama.Attributed;

public sealed record RecipeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }

}
