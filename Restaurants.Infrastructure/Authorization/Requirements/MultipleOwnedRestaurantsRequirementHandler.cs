using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MultipleOwnedRestaurantsRequirementHandler
    : AuthorizationHandler<MultipleOwnedRestaurantsRequirement>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<MultipleOwnedRestaurantsRequirementHandler> _logger;
    private readonly IUserContext _userContext;

    public MultipleOwnedRestaurantsRequirementHandler(
        IRestaurantsRepository restaurantsRepository,
        ILogger<MultipleOwnedRestaurantsRequirementHandler> logger,
        IUserContext userContext
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _userContext = userContext;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MultipleOwnedRestaurantsRequirement requirement
    )
    {
        var currentUser = _userContext.GetCurrentUser();

        _logger.LogInformation(
            "User: {Email} - handeling MinimumCreatedResaurantsRequirement",
            currentUser.Email
        );

        var userOwnedResturants =
            await _restaurantsRepository.GetOwnedRestaurantsCountByUserIdAsync(currentUser!.Id);

        if (userOwnedResturants >= requirement.MinimumOwnedRestaurants)
        {
            _logger.LogInformation("Minimum resturants owned authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
