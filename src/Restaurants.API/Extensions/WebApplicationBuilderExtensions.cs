using Restaurants.API.Exceptions;
using Restaurants.API.Logging;
using Restaurants.API.Startup;
using Serilog;

namespace Restaurants.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddControllers();
        builder.Services.AddRestaurantsSwaggerDocument();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddErrorHandling();
        builder.Services.AddLoggings();

        builder.Host.UseSerilog(
            (context, configuration) => configuration.ReadFrom.Configuration(context.Configuration)
        );
    }
}
