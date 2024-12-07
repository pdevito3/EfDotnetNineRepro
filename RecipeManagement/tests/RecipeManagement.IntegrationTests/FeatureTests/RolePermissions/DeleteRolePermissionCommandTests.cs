namespace RecipeManagement.IntegrationTests.FeatureTests.RolePermissions;

using RecipeManagement.SharedTestHelpers.Fakes.RolePermission;
using RecipeManagement.Domain.RolePermissions.Features;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Threading.Tasks;

public class DeleteRolePermissionCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_rolepermission_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var rolePermission = new FakeRolePermissionBuilder().Build();
        await testingServiceScope.InsertAsync(rolePermission);

        // Act
        var command = new DeleteRolePermission.Command(rolePermission.Id);
        await testingServiceScope.SendAsync(command);
        var rolePermissionResponse = await testingServiceScope
            .ExecuteDbContextAsync(db => db.RolePermissions
                .CountAsync(r => r.Id == rolePermission.Id));

        // Assert
        rolePermissionResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_rolepermission_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteRolePermission.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_rolepermission_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var rolePermission = new FakeRolePermissionBuilder().Build();
        await testingServiceScope.InsertAsync(rolePermission);

        // Act
        var command = new DeleteRolePermission.Command(rolePermission.Id);
        await testingServiceScope.SendAsync(command);
        var deletedRolePermission = await testingServiceScope.ExecuteDbContextAsync(db => db.RolePermissions
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == rolePermission.Id));

        // Assert
        deletedRolePermission?.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task must_be_permitted()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        testingServiceScope.SetUserNotPermitted(Permissions.CanDeletePermissions);

        // Act
        var command = new DeleteRolePermission.Command(Guid.NewGuid());
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}