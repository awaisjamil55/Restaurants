using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Domain.Repositories;
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
        services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<RestaurantsDbContext>();

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
}
