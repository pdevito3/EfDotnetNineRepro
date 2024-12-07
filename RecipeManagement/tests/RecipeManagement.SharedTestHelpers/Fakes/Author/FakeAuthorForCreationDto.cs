namespace RecipeManagement.SharedTestHelpers.Fakes.Author;

using AutoBogus;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.Dtos;

public sealed class FakeAuthorForCreationDto : AutoFaker<AuthorForCreationDto>
{
    public FakeAuthorForCreationDto()
    {
    }
}