namespace RecipeManagement.Domain.Authors.Features;

using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using MediatR;

public static class DeleteAuthor
{
    public sealed record Command(Guid AuthorId) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recordToDelete = await dbContext.Authors
                .GetById(request.AuthorId, cancellationToken: cancellationToken);
            dbContext.Remove(recordToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}