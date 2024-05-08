using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        _restaurantRepositoryMock = new Mock<IRestaurantsRepository>();

        _handler = new UpdateRestaurantCommandHandler(_restaurantRepositoryMock.Object, _restaurantAuthorizationServiceMock.Object, _loggerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ForValidCommand_ShouldUpdateRestaurant()
    {
        // arrange

        var restaurantId = 1;
        var command = new UpdateRestaurantCommand
        {
            Id = restaurantId,
            Name = "Test Name",
            Description = "Test Description",
            HasDelivery = true
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "update name test",
            Description = "update desctiption tes"
        };

        _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(a => a.Authorize(restaurant, ResourceOperation.Update)).Returns(true);

        // act

        await _handler.Handle(command, CancellationToken.None);

        // assert

        _restaurantRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);


    }

    [Fact]
    public async Task Handle_ForNonExistingRestaurant_ShouldThrowEntityNotFoundException()
    {
        // arrange

        var restaurantId = 2;
        var request = new UpdateRestaurantCommand
        {
            Id = restaurantId,
            Name = "Test Name",
            Description = "Test Description",
            HasDelivery = true
        };

        _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync((Restaurant?)null);

        // act

        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // assert

        await act.Should().ThrowAsync<EntityNotFoundException>().WithMessage($"Restaurant {restaurantId} doesn't exist");
    }

    [Fact]
    public async Task Handle_ForUnauthorizedUser_ShouldThrowForbidException()
    {
        // arrange

        var restaurantId = 2;
        var request = new UpdateRestaurantCommand
        {
            Id = restaurantId,
            Name = "Test Name",
            Description = "Test Description",
            HasDelivery = true
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId
        };

        _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId)).ReturnsAsync((Restaurant?)null);

        _restaurantAuthorizationServiceMock.Setup(a => a.Authorize(restaurant, ResourceOperation.Update)).Returns(false);

        // act

        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // assert

        await act.Should().ThrowAsync<ForbidException>();
    }
}