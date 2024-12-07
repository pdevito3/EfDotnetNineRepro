namespace RecipeManagement.Domain.Authors.Models;

using Destructurama.Attributed;

public sealed record AuthorForUpdate
{
    [LogMasked]
    public string Name { get; set; }
    public decimal Ownership { get; set; }
    public string PrimaryEmail { get; set; }
}
