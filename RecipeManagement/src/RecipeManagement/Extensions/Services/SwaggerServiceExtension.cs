namespace RecipeManagement.Extensions.Services;

using Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerServiceExtension
{
    public static void AddSwaggerExtension(this IServiceCollection services, 
        IConfiguration configuration)
    {
        var authOptions = configuration.GetAuthOptions();
        services.AddSwaggerGen(config =>
        {
            config.CustomSchemaIds(type => type.ToString().Replace("+", "."));
            config.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date"
            });

            config.IncludeXmlComments(string.Format(@$"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}RecipeManagement.WebApi.xml"));
        });
    }
}
