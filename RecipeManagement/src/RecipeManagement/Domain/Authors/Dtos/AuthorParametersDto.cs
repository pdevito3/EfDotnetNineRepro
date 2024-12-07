namespace RecipeManagement.Domain.Authors.Dtos;

using RecipeManagement.Resources;

public sealed class AuthorParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
