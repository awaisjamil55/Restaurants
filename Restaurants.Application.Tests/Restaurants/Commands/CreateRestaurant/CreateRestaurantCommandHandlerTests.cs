using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandHandlerTests
{
    [Fact]
    public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
    {
        // arrange

        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(repo => repo.Create(It.IsAny<Restaurant>())).ReturnsAsync(1);

        var mapperMock = new Mock<IMapper>();

        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();

        mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);

        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("Id1", "test@testing.com", [], null, null, null);
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var commandHandler = new CreateRestaurantCommandHandler(
            restaurantRepositoryMock.Object,
            new Mock<ILogger<CreateRestaurantCommandHandler>>().Object,
            mapperMock.Object,
            userContextMock.Object
        );

        // act

        var result = await commandHandler.Handle(command, CancellationToken.None);

        // assert

        result.Should().Be(1);
        restaurant.OwnerId.Should().Be("Id1");
        restaurantRepositoryMock.Verify(r => r.Create(restaurant), Times.Once);
    }
}
