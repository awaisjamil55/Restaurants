using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        //RegisterServices(services);

        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly))
            .AddAutoMapper(applicationAssembly)
            .AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();
    }

    /// <summary>
    /// Register all the services
    /// </summary>
    /// <param name="service"></param>
    //private static void RegisterServices(IServiceCollection services) =>
    //    services.AddScoped<IRestaurantsService, RestaurantsService>();
}
