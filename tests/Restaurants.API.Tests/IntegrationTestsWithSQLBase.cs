using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Seeders;
using Xunit;

namespace Restaurants.API.Tests;

public class IntegrationTestsWithSQLBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected WebApplicationFactory<Program> _factory;
    protected HttpClient _client;

    protected readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock = new();
    protected readonly Mock<IRestaurantSeeder> _restaurantSeederMock = new();

    public IntegrationTestsWithSQLBase(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                ConfigureTestServices(services);
            });
        });

        _client = _factory.CreateClient();
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

        services.Replace(
            ServiceDescriptor.Scoped(
                typeof(IRestaurantsRepository),
                _ => _restaurantRepositoryMock.Object
            )
        );

        services.Replace(
            ServiceDescriptor.Scoped(typeof(IRestaurantSeeder), _ => _restaurantSeederMock.Object)
        );
    }

    protected async Task<T?> Deserialize<T>(HttpResponseMessage response) =>
        await response.Content.ReadFromJsonAsync<T>();
}
