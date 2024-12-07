namespace RecipeManagement.IntegrationTests.FeatureTests.Users;

using RecipeManagement.Domain.Roles;
using RecipeManagement.SharedTestHelpers.Fakes.User;
using RecipeManagement.Domain.Users.Features;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class AddRemoveUserRoleTests : TestBase
{    
    [Fact]
    public async Task can_add_and_remove_role()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var faker = new Faker();
        var fakeUserOne = new FakeUserBuilder().Build();
        await testingServiceScope.InsertAsync(fakeUserOne);

        var user = await testingServiceScope.ExecuteDbContextAsync(db => db.Users
            .FirstOrDefaultAsync(u => u.Id == fakeUserOne.Id));
        var id = user.Id;
        var role = faker.PickRandom(Role.ListNames());

        // Act - Add
        var command = new AddUserRole.Command(id, role);
        await testingServiceScope.SendAsync(command);
        var updatedUser = await testingServiceScope.ExecuteDbContextAsync(db => db.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id));

        // Assert - Add
        updatedUser.Roles.Count.Should().Be(1);
        updatedUser.Roles.FirstOrDefault().Role.Value.Should().Be(role);
        
        // Act - Remove
        var removeCommand = new RemoveUserRole.Command(id, role);
        await testingServiceScope.SendAsync(removeCommand);
        
        // Assert - Remove
        updatedUser = await testingServiceScope.ExecuteDbContextAsync(db => db.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id));
        updatedUser.Roles.Count.Should().Be(0);
    }
}