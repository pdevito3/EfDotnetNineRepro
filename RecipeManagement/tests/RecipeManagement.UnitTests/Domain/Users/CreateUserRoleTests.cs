namespace RecipeManagement.UnitTests.Domain.Users;

using RecipeManagement.SharedTestHelpers.Fakes.User;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Roles;
using RecipeManagement.Domain.Users.DomainEvents;
using Bogus;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class CreateUserRoleTests
{
    private readonly Faker _faker;

    public CreateUserRoleTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_userRole()
    {
        // Arrange
        var user = new FakeUserBuilder().Build();
        var role = _faker.PickRandom(Role.ListNames());
        
        // Act
        var fakeUserRole = UserRole.Create(user, new Role(role));

        // Assert
        fakeUserRole.User.Id.Should().Be(user.Id);
        fakeUserRole.Role.Should().Be(new Role(role));
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var user = new FakeUserBuilder().Build();
        var role = _faker.PickRandom(Role.ListNames());
        
        // Act
        var fakeUserRole = UserRole.Create(user, new Role(role));

        // Assert
        fakeUserRole.DomainEvents.Count.Should().Be(1);
        fakeUserRole.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserRolesUpdated));
    }
}