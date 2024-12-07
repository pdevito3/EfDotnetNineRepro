namespace RecipeManagement.Domain.Ingredients.Dtos;

using Destructurama.Attributed;

public sealed record IngredientDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Quantity { get; set; }
    public DateTime? ExpiresOn { get; set; }
    public DateTimeOffset? BestTimeOfDay { get; set; }
    public string Measure { get; set; }
}
