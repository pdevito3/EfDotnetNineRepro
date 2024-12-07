namespace RecipeManagement.UnitTests.Domain.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class CreateAuthorTests
{
    private readonly Faker _faker;

    public CreateAuthorTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_author()
    {
        // Arrange
        var authorToCreate = new FakeAuthorForCreation().Generate();
        
        // Act
        var author = Author.Create(authorToCreate);

        // Assert
        author.Name.Should().Be(authorToCreate.Name);
        author.PrimaryEmail.Should().Be(authorToCreate.PrimaryEmail);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var authorToCreate = new FakeAuthorForCreation().Generate();
        
        // Act
        var author = Author.Create(authorToCreate);

        // Assert
        author.DomainEvents.Count.Should().Be(1);
        author.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AuthorCreated));
    }
}