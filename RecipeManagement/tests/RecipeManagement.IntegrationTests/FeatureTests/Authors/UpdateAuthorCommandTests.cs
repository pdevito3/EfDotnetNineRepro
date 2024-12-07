namespace RecipeManagement.IntegrationTests.FeatureTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Domain.Authors.Features;
using Domain;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UpdateAuthorCommandTests : TestBase
{
    [Fact]
    public async Task can_update_existing_author_in_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var author = new FakeAuthorBuilder().Build();
        await testingServiceScope.InsertAsync(author);
        var updatedAuthorDto = new FakeAuthorForUpdateDto().Generate();

        // Act
        var command = new UpdateAuthor.Command(author.Id, updatedAuthorDto);
        await testingServiceScope.SendAsync(command);
        var updatedAuthor = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Authors
                .FirstOrDefaultAsync(a => a.Id == author.Id));

        // Assert
        updatedAuthor.Name.Should().Be(updatedAuthorDto.Name);
        updatedAuthor.PrimaryEmail.Should().Be(updatedAuthorDto.PrimaryEmail);
        updatedAuthor.Ownership.Value.Should().BeApproximately(updatedAuthorDto.Ownership, 0.005M);
    }
}