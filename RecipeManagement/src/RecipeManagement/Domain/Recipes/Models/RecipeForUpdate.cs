namespace RecipeManagement.Domain.Recipes.Models;

using Destructurama.Attributed;

public sealed record RecipeForUpdate
{
    public string Title { get; set; }
    public string Directions { get; set; }
    public int? Rating { get; set; }
    public DateOnly? DateOfOrigin { get; set; }
    public bool HaveMadeItMyself { get; set; }

}
