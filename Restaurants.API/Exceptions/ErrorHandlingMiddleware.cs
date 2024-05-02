using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Exceptions;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (EntityNotFoundException entityNotFoundException)
        {
            _logger.LogWarning(entityNotFoundException.Message);

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(entityNotFoundException.Message);
        }
        catch (ForbidException)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access forbidden");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}
