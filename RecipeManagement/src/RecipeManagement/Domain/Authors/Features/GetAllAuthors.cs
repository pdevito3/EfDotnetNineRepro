namespace RecipeManagement.Domain.Authors.Features;

using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using RecipeManagement.Resources;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetAllAuthors
{
    public sealed record Query() : IRequest<List<AuthorDto>>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, List<AuthorDto>>
    {
        public async Task<List<AuthorDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return dbContext.Authors
                .AsNoTracking()
                .ToAuthorDtoQueryable()
                .ToList();
        }
    }
}