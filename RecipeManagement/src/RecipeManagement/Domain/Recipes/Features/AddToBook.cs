namespace RecipeManagement.Domain.Recipes.Features;

using MassTransit;
using SharedKernel.Messages;
using System.Threading.Tasks;
using RecipeManagement.Databases;

public sealed class AddToBook(RecipesDbContext dbContext) : IConsumer<IImportRecipe>
{
    public Task Consume(ConsumeContext<IImportRecipe> context)
    {
        // do work here

        return Task.CompletedTask;
    }
}