namespace Restaurants.API.Exceptions;

public static class ServiceCollectionExtensions
{
    public static void AddErrorHandling(this IServiceCollection services) =>
        services.AddScoped<ErrorHandlingMiddleware>();
}
