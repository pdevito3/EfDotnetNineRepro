namespace RecipeManagement.Extensions.Services;

using Resources;
using System.Text.Json.Serialization;
using Serilog;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Resources;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

public static class WebAppServiceConfiguration
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RecipeManagementOptions>(builder.Configuration.GetSection(RecipeManagementOptions.SectionName));

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddSingleton(Log.Logger);

        // TODO update CORS for your env
        builder.Services.AddInfrastructure(builder.Environment, builder.Configuration);

        builder.Services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        builder.Services.AddMvc();

        builder.Services.AddSwaggerExtension(builder.Configuration);
    }
}
