namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Domain.Recipes.Models;
using MediatR;

public static class AddRecipe
{
    public sealed record Command(RecipeForCreationDto RecipeToAdd) : IRequest<RecipeDto>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command, RecipeDto>
    {
        public async Task<RecipeDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var recipeToAdd = new RecipeForCreation()
            {
                Title = request.RecipeToAdd.Title
            };
            var recipe = Recipe.Create(recipeToAdd);

            await dbContext.Recipes.AddAsync(recipe, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new RecipeDto()
            {
                Id = recipe.Id,
                Title = recipe.Title
            };
        }
    }
}