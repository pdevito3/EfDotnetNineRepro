namespace RecipeManagement.UnitTests.Domain.Users;

using RecipeManagement.Domain.Users.DomainEvents;
using RecipeManagement.Domain.Emails;
using RecipeManagement.Domain.Users;
using RecipeManagement.Resources;
using RecipeManagement.Domain.Users.Models;
using RecipeManagement.SharedTestHelpers.Fakes.User;
using Bogus;
using ValidationException = RecipeManagement.Exceptions.ValidationException;

public class CreateUserTests
{
    private readonly Faker _faker;

    public CreateUserTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_user()
    {
        // Arrange
        var toCreate = new FakeUserForCreation().Generate();

        // Act
        var newUser = User.Create(toCreate);
        
        // Assert
        newUser.Identifier.Should().Be(toCreate.Identifier);
        newUser.FirstName.Should().Be(toCreate.FirstName);
        newUser.LastName.Should().Be(toCreate.LastName);
        newUser.Email.Should().Be(new Email(toCreate.Email));
        newUser.Username.Should().Be(toCreate.Username);
    }
    
    [Fact]
    public void can_NOT_create_user_without_identifier()
    {
        // Arrange
        var toCreate = new FakeUserForCreation().Generate();
        toCreate.Identifier = null;
        var newUser = () => User.Create(toCreate);

        // Act + Assert
        newUser.Should().Throw<ValidationException>();
    }
    
    [Fact]
    public void can_NOT_create_user_with_whitespace_identifier()
    {
        // Arrange
        var toCreate = new FakeUserForCreation().Generate();
        toCreate.Identifier = " ";
        var newUser = () => User.Create(toCreate);

        // Act + Assert
        newUser.Should().Throw<ValidationException>();
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var toCreate = new FakeUserForCreation().Generate();

        // Act
        var newUser = User.Create(toCreate);

        // Assert
        newUser.DomainEvents.Count.Should().Be(1);
        newUser.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserCreated));
    }
}