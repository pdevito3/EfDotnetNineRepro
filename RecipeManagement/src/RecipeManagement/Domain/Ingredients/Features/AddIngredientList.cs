namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Databases;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients.Models;
using RecipeManagement.Exceptions;
using Mappings;
using MediatR;

public static class AddIngredientList
{
    public sealed record Command(IEnumerable<IngredientForCreationDto> IngredientListToAdd, Guid RecipeId) : IRequest<List<IngredientDto>>;

    public sealed class Handler(RecipesDbContext dbContext)
        : IRequestHandler<Command, List<IngredientDto>>
    {
        public async Task<List<IngredientDto>> Handle(Command command, CancellationToken cancellationToken)
        {
            var recipe = await dbContext.Recipes.GetById(command.RecipeId, cancellationToken);

            var ingredientListToAdd = command.IngredientListToAdd.ToList();
            var ingredientList = new List<Ingredient>();
            foreach (var ingredient in ingredientListToAdd)
            {
                var ingredientForCreation = ingredient.ToIngredientForCreation();
                var ingredientToAdd = Ingredient.Create(ingredientForCreation);
                ingredientList.Add(ingredientToAdd);
                recipe.AddIngredient(ingredientToAdd);
            }

            // if you have large datasets to add in bulk and have performance concerns, there 
            // are additional methods that could be leveraged in your repository instead (e.g. SqlBulkCopy)
            // https://timdeschryver.dev/blog/faster-sql-bulk-inserts-with-csharp#table-valued-parameter 
            await dbContext.Ingredients.AddRangeAsync(ingredientList, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return ingredientList
                .Select(i => i.ToIngredientDto())
                .ToList();
        }
    }
}