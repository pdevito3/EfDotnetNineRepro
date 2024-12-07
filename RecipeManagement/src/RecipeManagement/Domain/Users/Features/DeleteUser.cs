namespace RecipeManagement.Domain.Users.Features;

using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MediatR;

public static class DeleteUser
{
    public sealed record Command(Guid UserId) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recordToDelete = await dbContext.Users
                .GetById(request.UserId, cancellationToken: cancellationToken);
            dbContext.Remove(recordToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}