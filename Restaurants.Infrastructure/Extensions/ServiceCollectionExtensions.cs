using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        RegisterDbContext(services, configuration);
        RegisterIdentity(services);
        RegisterSeeders(services);
        RegisterRepositories(services);
        RegisterRequirments(services);
        RegisterPolicies(services);
        RegisterServices(services);
    }

    /// <summary>
    /// Register DbContext
    /// </summary>
    /// <param name="service"></param>
    private static void RegisterDbContext(
        IServiceCollection services,
        IConfiguration configuration
    ) =>
        services.AddDbContext<RestaurantsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("RestaurantsDb"))
        );

    /// <summary>
    /// Register AspNetCore Identity
    /// </summary>
    /// <param name="service"></param>
    private static void RegisterIdentity(IServiceCollection services) =>
        services
            .AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantsDbContext>();

    /// <summary>
    /// Register Authorization Requirments
    /// </summary>
    /// <param name="service"></param>
    private static void RegisterRequirments(IServiceCollection services) =>
        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();

    /// <summary>
    /// Register Policies
    /// </summary>
    /// <param name="service"></param>
    private static void RegisterPolicies(IServiceCollection services) =>
        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                Policies.HavePassport,
                builder => builder.RequireClaim(AppClaimTypes.HavePassport, "Yes")
            )
            .AddPolicy(
                Policies.MinimumAge18,
                builder => builder.AddRequirements(new MinimumAgeRequirement(18))
            );

    /// <summary>
    /// Register Seeders
    /// </summary>
    /// <param name="service"></param>
    private static void RegisterSeeders(IServiceCollection services) =>
        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();

    /// <summary>
    /// Register all the repositories
    /// </summary>
    /// <param name="service"></param>
    private static void RegisterRepositories(IServiceCollection services) =>
        services
            .AddScoped<IRestaurantsRepository, RestaurantsRepository>()
            .AddScoped<IDishesRepository, DishesRepository>();

    /// <summary>
    /// Register Services
    /// </summary>
    /// <param name="service"></param>
    private static void RegisterServices(IServiceCollection services) =>
        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
}
