namespace RecipeManagement.IntegrationTests.FeatureTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteAuthorCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_author_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var author = new FakeAuthorBuilder().Build();
        await testingServiceScope.InsertAsync(author);

        // Act
        var command = new DeleteAuthor.Command(author.Id);
        await testingServiceScope.SendAsync(command);
        var authorResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.Authors
                .CountAsync(a => a.Id == author.Id));

        // Assert
        authorResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_author_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteAuthor.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_author_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var author = new FakeAuthorBuilder().Build();
        await testingServiceScope.InsertAsync(author);

        // Act
        var command = new DeleteAuthor.Command(author.Id);
        await testingServiceScope.SendAsync(command);
        var deletedAuthor = await testingServiceScope.ExecuteDbContextAsync(db => db.Authors
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == author.Id));

        // Assert
        deletedAuthor?.IsDeleted.Should().BeTrue();
    }
}