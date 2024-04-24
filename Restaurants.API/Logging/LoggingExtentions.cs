namespace Restaurants.API.Logging;

public static class LoggingExtentions
{
    public static IApplicationBuilder UseLogging(this IApplicationBuilder applicationBuilder) =>
        applicationBuilder.UseMiddleware<RequestTimeLoggingMiddleware>();
}
