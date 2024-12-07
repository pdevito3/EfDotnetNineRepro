namespace RecipeManagement.Domain.Authors.Dtos;

using Destructurama.Attributed;

public sealed record AuthorForCreationDto
{
    [LogMasked]
    public string Name { get; set; }
    public decimal Ownership { get; set; }
    public string PrimaryEmail { get; set; }
}
