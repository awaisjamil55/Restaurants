namespace Restaurants.API.Exceptions;

public static class ExceptionExtensions
{
    public static IApplicationBuilder UseErrorHandling(
        this IApplicationBuilder applicationBuilder
    ) => applicationBuilder.UseMiddleware<ErrorHandlingMiddleware>();
}
