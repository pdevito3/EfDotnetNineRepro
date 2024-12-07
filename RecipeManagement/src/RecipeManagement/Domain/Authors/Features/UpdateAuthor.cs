namespace RecipeManagement.Domain.Authors.Features;

using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Services;
using RecipeManagement.Domain.Authors.Models;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class UpdateAuthor
{
    public sealed record Command(Guid AuthorId, AuthorForUpdateDto UpdatedAuthorData) : IRequest;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var authorToUpdate = await dbContext.Authors.GetById(request.AuthorId, cancellationToken: cancellationToken);
            var authorToAdd = request.UpdatedAuthorData.ToAuthorForUpdate();
            authorToUpdate.Update(authorToAdd);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}