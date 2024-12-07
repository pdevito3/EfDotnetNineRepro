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

public static class GetAuthorList
{
    public sealed record Query(AuthorParametersDto QueryParameters) : IRequest<PagedList<AuthorDto>>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Query, PagedList<AuthorDto>>
    {
        public async Task<PagedList<AuthorDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = dbContext.Authors.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToAuthorDtoQueryable();

            return await PagedList<AuthorDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}