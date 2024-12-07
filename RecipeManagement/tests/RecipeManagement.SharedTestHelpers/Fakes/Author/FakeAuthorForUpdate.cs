namespace RecipeManagement.SharedTestHelpers.Fakes.Author;

using AutoBogus;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.Models;

public sealed class FakeAuthorForUpdate : AutoFaker<AuthorForUpdate>
{
    public FakeAuthorForUpdate()
    {
    }
}