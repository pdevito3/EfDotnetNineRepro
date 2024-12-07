namespace RecipeManagement.SharedTestHelpers.Fakes.Author;

using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.Models;

public class FakeAuthorBuilder
{
    private AuthorForCreation _creationData = new FakeAuthorForCreation().Generate();

    public FakeAuthorBuilder WithModel(AuthorForCreation model)
    {
        _creationData = model;
        return this;
    }
    
    public FakeAuthorBuilder WithName(string name)
    {
        _creationData.Name = name;
        return this;
    }
    
    public FakeAuthorBuilder WithPrimaryEmail(string primaryEmail)
    {
        _creationData.PrimaryEmail = primaryEmail;
        return this;
    }
    
    public Author Build()
    {
        var result = Author.Create(_creationData);
        return result;
    }
}