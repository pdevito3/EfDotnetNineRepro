namespace RecipeManagement.IntegrationTests.FeatureTests.Users;

using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.User;
using RecipeManagement.Domain.Users.Features;
using Domain;
using System.Threading.Tasks;

public class UserListQueryTests : TestBase
{
    
    [Fact]
    public async Task can_get_user_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var userOne = new FakeUserBuilder().Build();
        var userTwo = new FakeUserBuilder().Build();
        var queryParameters = new UserParametersDto();

        await testingServiceScope.InsertAsync(userOne, userTwo);

        // Act
        var query = new GetUserList.Query(queryParameters);
        var users = await testingServiceScope.SendAsync(query);

        // Assert
        users.Count.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task must_be_permitted()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        testingServiceScope.SetUserNotPermitted(Permissions.CanGetUsers);
        var queryParameters = new UserParametersDto();

        // Act
        var command = new GetUserList.Query(queryParameters);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}