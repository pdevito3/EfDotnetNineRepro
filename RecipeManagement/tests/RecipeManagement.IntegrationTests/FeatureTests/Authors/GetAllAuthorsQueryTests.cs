namespace RecipeManagement.IntegrationTests.FeatureTests.Authors;

using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors.Features;
using Domain;
using System.Threading.Tasks;

public class GetAllAuthorsQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_all_authors()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var authorOne = new FakeAuthorBuilder().Build();
        var authorTwo = new FakeAuthorBuilder().Build();

        await testingServiceScope.InsertAsync(authorOne, authorTwo);

        // Act
        var query = new GetAllAuthors.Query();
        var authors = await testingServiceScope.SendAsync(query);

        // Assert
        authors.Count.Should().BeGreaterThanOrEqualTo(2);
        authors.FirstOrDefault(x => x.Id == authorOne.Id).Should().NotBeNull();
        authors.FirstOrDefault(x => x.Id == authorTwo.Id).Should().NotBeNull();
    }
}