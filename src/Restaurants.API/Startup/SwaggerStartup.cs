using Microsoft.OpenApi.Models;

namespace Restaurants.API.Startup;

internal static class SwaggerStartup
{
    internal static void AddRestaurantsSwaggerDocument(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition(
                "bearerAuth",
                new OpenApiSecurityScheme { Type = SecuritySchemeType.Http, Scheme = "Bearer" }
            );
            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth"
                            }
                        },
                        []
                    }
                }
            );
        });
    }

    internal static void UseRestaurantsSwaggerUI(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
