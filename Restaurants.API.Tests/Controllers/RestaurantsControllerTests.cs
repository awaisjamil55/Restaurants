using System.Net;
using Azure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Restaurants.API.Tests;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IntegrationTestsWithSQLBase
{
    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
        : base(factory) { }

    [Fact]
    public async void GetById_ForNonExistingId_ShouldReturn404NotFound()
    {
        // arrange
        var restaurantId = 213;

        _restaurantRepositoryMock
            .Setup(r => r.GetByIdAsync(restaurantId))
            .ReturnsAsync((Restaurant?)null);

        // act

        var response = await _client.GetAsync($"/api/restaurants/{restaurantId}");

        // assert

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void GetById_ForExistingId_ShouldReturn200Ok()
    {
        // arrange
        var restaurantId = 213;
        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = "test",
            Description = "test description",
            HasDelivery = true,
            ContactEmail = "asd",
            ContactNumber = null
        };

        _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);

        // act

        var response = await _client.GetAsync($"/api/restaurants/{restaurantId}");

        // assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var restaurantDto = await Deserialize<RestaurantDto>(response);

        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurantId);
        restaurantDto.Name.Should().Be("test");
        restaurantDto.Description.Should().Be("test description");
        restaurantDto.HasDelivery.Should().BeTrue();
    }

    [Fact]
    public async void GetAll_ForValidRequest_Returns200Ok()
    {
        // act

        var response = await _client.GetAsync($"/api/restaurants?Offset=0&Limit=20");

        // assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async void GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        // act

        var response = await _client.GetAsync($"/api/restaurants?Offset=-1&Limit=20");

        // assert

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
