namespace RecipeManagement.Domain.Recipes.Dtos;

using Destructurama.Attributed;

public sealed record RecipeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Directions { get; set; }
    public DateOnly? DateOfOrigin { get; set; }
    public bool HaveMadeItMyself { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();

}
