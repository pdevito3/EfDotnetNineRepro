namespace RecipeManagement.UnitTests.Domain.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.DomainEvents;
using Bogus;
using FluentAssertions.Extensions;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class UpdateAuthorTests
{
    private readonly Faker _faker;

    public UpdateAuthorTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_update_author()
    {
        // Arrange
        var author = new FakeAuthorBuilder().Build();
        var updatedAuthor = new FakeAuthorForUpdate().Generate();
        
        // Act
        author.Update(updatedAuthor);

        // Assert
        author.Name.Should().Be(updatedAuthor.Name);
        author.PrimaryEmail.Should().Be(updatedAuthor.PrimaryEmail);
    }
    
    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var author = new FakeAuthorBuilder().Build();
        var updatedAuthor = new FakeAuthorForUpdate().Generate();
        author.DomainEvents.Clear();
        
        // Act
        author.Update(updatedAuthor);

        // Assert
        author.DomainEvents.Count.Should().Be(1);
        author.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AuthorUpdated));
    }
}