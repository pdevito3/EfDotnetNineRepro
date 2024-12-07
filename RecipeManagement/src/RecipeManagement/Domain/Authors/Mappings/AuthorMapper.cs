namespace RecipeManagement.Domain.Authors.Mappings;

using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Domain.Authors.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class AuthorMapper
{
    public static partial AuthorForCreation ToAuthorForCreation(this AuthorForCreationDto authorForCreationDto);
    public static partial AuthorForUpdate ToAuthorForUpdate(this AuthorForUpdateDto authorForUpdateDto);

    [MapProperty(new[] { nameof(Author.Ownership), nameof(Author.Ownership.Value) }, new[] { nameof(AuthorDto.Ownership) })]
    public static partial AuthorDto ToAuthorDto(this Author author);

    [MapProperty(new[] { nameof(Author.Ownership), nameof(Author.Ownership.Value) }, new[] { nameof(AuthorDto.Ownership) })]
    public static partial IQueryable<AuthorDto> ToAuthorDtoQueryable(this IQueryable<Author> author);
}
