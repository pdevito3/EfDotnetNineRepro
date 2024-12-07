namespace RecipeManagement.Domain.Recipes.Features;

using SharedKernel.Messages;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using RecipeManagement.Databases;

public static class ImportRecipeProducer
{
    public sealed record ImportRecipeProducerCommand : IRequest;

    public sealed class Handler(RecipesDbContext dbContext, IPublishEndpoint publishEndpoint) : IRequestHandler<ImportRecipeProducerCommand>
    {

        public async Task Handle(ImportRecipeProducerCommand request, CancellationToken cancellationToken)
        {
            var message = new ImportRecipe
            {
                // map content to message here
            };
            await publishEndpoint.Publish<IImportRecipe>(message, cancellationToken);
        }
    }
}