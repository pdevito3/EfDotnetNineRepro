namespace RecipeManagement.Domain.Recipes.Dtos;


public sealed record RecipeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }

}
