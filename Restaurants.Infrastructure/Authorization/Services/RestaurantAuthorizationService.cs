using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService : IRestaurantAuthorizationService
{
    private readonly IUserContext _userContext;
    private readonly ILogger<RestaurantAuthorizationService> _logger;

    public RestaurantAuthorizationService(
        IUserContext userContext,
        ILogger<RestaurantAuthorizationService> logger
    )
    {
        _userContext = userContext;
        _logger = logger;
    }

    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var currentUser = _userContext.GetCurrentUser();

        _logger.LogInformation(
            "Authorization use {UserEmail}, to {Operation} for restaurant {RestaurantName}",
            currentUser.Email,
            resourceOperation,
            restaurant.Name
        );

        if (
            resourceOperation == ResourceOperation.Read
            || resourceOperation == ResourceOperation.Create
        )
        {
            _logger.LogInformation("Read/Create operation - authorization succeeded");
            return true;
        }

        if (resourceOperation == ResourceOperation.Delete && currentUser.IsInRole(UserRoles.Admin))
        {
            _logger.LogInformation("Admin user/delete operation - authorization succeeded");
            return true;
        }

        if (
            (
                resourceOperation == ResourceOperation.Update
                || resourceOperation == ResourceOperation.Delete
            )
            && currentUser.Id == restaurant.OwnerId
        )
        {
            _logger.LogInformation("Restaurant owner - authorization succeeded");
            return true;
        }

        return false;
    }
}
