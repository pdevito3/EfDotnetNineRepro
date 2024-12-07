namespace RecipeManagement.IntegrationTests.FeatureTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class AuthorQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_author_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var authorOne = new FakeAuthorBuilder().Build();
        await testingServiceScope.InsertAsync(authorOne);

        // Act
        var query = new GetAuthor.Query(authorOne.Id);
        var author = await testingServiceScope.SendAsync(query);

        // Assert
        author.Name.Should().Be(authorOne.Name);
        author.PrimaryEmail.Should().Be(authorOne.PrimaryEmail);
        author.Ownership.Should().BeApproximately(authorOne.Ownership.Value, 0.005M);
    }

    [Fact]
    public async Task get_author_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetAuthor.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}