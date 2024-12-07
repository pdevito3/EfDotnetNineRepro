namespace RecipeManagement.Domain.Ingredients.Models;

using Destructurama.Attributed;

public sealed record IngredientForCreation
{
    public string Name { get; set; }
    public string Quantity { get; set; }
    public DateTime? ExpiresOn { get; set; }
    public DateTimeOffset? BestTimeOfDay { get; set; }
    public string Measure { get; set; }
    public decimal AverageCost { get; set; }
}
