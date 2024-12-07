namespace RecipeManagement.Domain.Recipes.Dtos;

using Destructurama.Attributed;

public sealed record RecipeForCreationDto
{
    public string Title { get; set; }
    public string Visibility { get; set; }
    public string Directions { get; set; }
    public int? Rating { get; set; }
    public DateOnly? DateOfOrigin { get; set; }
    public bool HaveMadeItMyself { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();

}
