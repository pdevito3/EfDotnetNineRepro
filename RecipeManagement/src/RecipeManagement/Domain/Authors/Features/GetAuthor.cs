namespace RecipeManagement.Domain.Authors.Features;

using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetAuthor
{
    public sealed record Query(Guid AuthorId) : IRequest<AuthorDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, AuthorDto>
    {
        public async Task<AuthorDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Authors
                .AsNoTracking()
                .GetById(request.AuthorId, cancellationToken);
            return result.ToAuthorDto();
        }
    }
}