namespace Restaurants.API.Logging;

public static class ServiceCollectionExtensions
{
    public static void AddLoggings(this IServiceCollection services) =>
        services.AddScoped<RequestTimeLoggingMiddleware>();
}
