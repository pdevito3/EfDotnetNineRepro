namespace RecipeManagement.IntegrationTests.FeatureTests.Authors;

using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors.Features;
using Domain;
using System.Threading.Tasks;

public class AuthorListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_author_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var authorOne = new FakeAuthorBuilder().Build();
        var authorTwo = new FakeAuthorBuilder().Build();
        var queryParameters = new AuthorParametersDto();

        await testingServiceScope.InsertAsync(authorOne, authorTwo);

        // Act
        var query = new GetAuthorList.Query(queryParameters);
        var authors = await testingServiceScope.SendAsync(query);

        // Assert
        authors.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}