using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests;

public class MultipleOwnedRestaurantsRequirementHandlerTests
{
    [Fact]
    public async Task HandleRequirementAsync_UserOwnedMultipleRestaurants_ShouldSucceed()
    {
        // arrange

        var currentUser = new CurrentUser("1", "test@testing.com", [], null, null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new() { OwnerId = currentUser.Id },
            new() { OwnerId = currentUser.Id },
            new() { OwnerId = currentUser.Id },
            new() { OwnerId = "2" }
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();

        restaurantsRepositoryMock
            .Setup(r => r.GetAllAsync(0, restaurants.Count()))
            .ReturnsAsync(restaurants);

        restaurantsRepositoryMock
            .Setup(r => r.GetOwnedRestaurantsCountByUserIdAsync(currentUser.Id))
            .ReturnsAsync(3);

        var requirement = new MultipleOwnedRestaurantsRequirement(2);
        var handler = new MultipleOwnedRestaurantsRequirementHandler(
            restaurantsRepositoryMock.Object,
            new Mock<ILogger<MultipleOwnedRestaurantsRequirementHandler>>().Object,
            userContextMock.Object
        );

        var context = new AuthorizationHandlerContext([requirement], null, null);

        // act

        await handler.HandleAsync(context);

        // assert

        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_UserDoesNotOwnMultipleRestaurants_ShouldFail()
    {
        // arrange

        var currentUser = new CurrentUser("1", "test@testing.com", [], null, null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new() { OwnerId = currentUser.Id },
            new() { OwnerId = "2" }
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();

        restaurantsRepositoryMock
            .Setup(r => r.GetAllAsync(0, restaurants.Count()))
            .ReturnsAsync(restaurants);

        restaurantsRepositoryMock
            .Setup(r => r.GetOwnedRestaurantsCountByUserIdAsync(currentUser.Id))
            .ReturnsAsync(1);

        var requirement = new MultipleOwnedRestaurantsRequirement(2);
        var handler = new MultipleOwnedRestaurantsRequirementHandler(
            restaurantsRepositoryMock.Object,
            new Mock<ILogger<MultipleOwnedRestaurantsRequirementHandler>>().Object,
            userContextMock.Object
        );

        var context = new AuthorizationHandlerContext([requirement], null, null);

        // act

        await handler.HandleAsync(context);

        // assert

        context.HasFailed.Should().BeTrue();
    }
}
