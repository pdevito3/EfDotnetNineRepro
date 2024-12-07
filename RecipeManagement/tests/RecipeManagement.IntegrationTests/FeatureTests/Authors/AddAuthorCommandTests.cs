namespace RecipeManagement.IntegrationTests.FeatureTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RecipeManagement.Domain.Authors.Features;

public class AddAuthorCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_author_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var authorOne = new FakeAuthorForCreationDto().Generate();

        // Act
        var command = new AddAuthor.Command(authorOne);
        var authorReturned = await testingServiceScope.SendAsync(command);
        var authorCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Authors
            .FirstOrDefaultAsync(a => a.Id == authorReturned.Id));

        // Assert
        authorReturned.Name.Should().Be(authorOne.Name);
        authorReturned.PrimaryEmail.Should().Be(authorOne.PrimaryEmail);
        authorReturned.Ownership.Should().BeApproximately(authorOne.Ownership, 0.005M);

        authorCreated.Name.Should().Be(authorOne.Name);
        authorCreated.PrimaryEmail.Should().Be(authorOne.PrimaryEmail);
        authorCreated.Ownership.Value.Should().BeApproximately(authorOne.Ownership, 0.005M);
    }
}