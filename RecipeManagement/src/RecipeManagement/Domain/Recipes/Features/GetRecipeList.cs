namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Databases;
using RecipeManagement.Exceptions;
using RecipeManagement.Resources;
using RecipeManagement.Domain;
using HeimGuard;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using QueryKit;
using QueryKit.Configuration;

public static class GetRecipeList
{
    public sealed record Query(RecipeParametersDto QueryParameters) : IRequest<PagedList<RecipeDto>>;

    public sealed class Handler(RecipesDbContext dbContext, IHeimGuardClient heimGuard)
        : IRequestHandler<Query, PagedList<RecipeDto>>
    {
        public async Task<PagedList<RecipeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            await heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadRecipes);

            var collection = dbContext.Recipes.AsNoTracking();

            var queryKitConfig = new CustomQueryKitConfiguration();
            var queryKitData = new QueryKitData()
            {
                Filters = request.QueryParameters.Filters,
                SortOrder = request.QueryParameters.SortOrder,
                Configuration = queryKitConfig
            };
            var appliedCollection = collection.ApplyQueryKit(queryKitData);
            var dtoCollection = appliedCollection.ToRecipeDtoQueryable();

            return await PagedList<RecipeDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}