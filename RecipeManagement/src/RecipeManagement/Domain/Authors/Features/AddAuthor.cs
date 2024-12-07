namespace RecipeManagement.Domain.Authors.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Domain.Authors.Models;
using RecipeManagement.Services;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class AddAuthor
{
    public sealed record Command(AuthorForCreationDto AuthorToAdd) : IRequest<AuthorDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command, AuthorDto>
    {
        public async Task<AuthorDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var authorToAdd = request.AuthorToAdd.ToAuthorForCreation();
            var author = Author.Create(authorToAdd);

            await dbContext.Authors.AddAsync(author, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return author.ToAuthorDto();
        }
    }
}