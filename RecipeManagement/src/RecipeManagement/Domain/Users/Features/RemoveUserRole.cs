namespace RecipeManagement.Domain.Users.Features;

using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using HeimGuard;
using MediatR;
using Roles;

public static class RemoveUserRole
{
    public sealed record Command(Guid UserId, string Role) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await dbContext.GetUserAggregate().GetById(request.UserId, cancellationToken);

            var roleToRemove = user.RemoveRole(new Role(request.Role));
            dbContext.UserRoles.Remove(roleToRemove);
            dbContext.Update(user);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}