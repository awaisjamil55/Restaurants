using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Users;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly))
            .AddAutoMapper(applicationAssembly)
            .AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();

        RegisterUserContext(services);
    }

    /// <summary>
    /// Register User Context
    /// </summary>
    /// <param name="service"></param>
    private static void RegisterUserContext(IServiceCollection services) =>
        services.AddScoped<IUserContext, UserContext>().AddHttpContextAccessor();
}
