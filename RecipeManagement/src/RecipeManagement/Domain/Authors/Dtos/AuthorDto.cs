namespace RecipeManagement.Domain.Authors.Dtos;

using Destructurama.Attributed;

public sealed record AuthorDto
{
    public Guid Id { get; set; }
    [LogMasked]
    public string Name { get; set; }
    public decimal Ownership { get; set; }
    public string PrimaryEmail { get; set; }
}
