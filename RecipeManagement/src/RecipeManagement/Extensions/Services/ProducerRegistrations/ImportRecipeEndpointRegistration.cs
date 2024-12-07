namespace RecipeManagement.Extensions.Services.ProducerRegistrations;

using MassTransit;
using MassTransit.RabbitMqTransport;
using SharedKernel.Messages;
using RabbitMQ.Client;

public static class ImportRecipeEndpointRegistration
{
    public static void ImportRecipeEndpoint(this IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.Message<ImportRecipe>(e => e.SetEntityName("import-recipe")); // name of the primary exchange
        cfg.Publish<ImportRecipe>(e => e.ExchangeType = ExchangeType.Fanout); // primary exchange type
    }
}