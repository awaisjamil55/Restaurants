using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class CreatedMultipleRestaurantsRequirementHandler
    : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<CreatedMultipleRestaurantsRequirementHandler> _logger;
    private readonly IUserContext _userContext;

    public CreatedMultipleRestaurantsRequirementHandler(
        IRestaurantsRepository restaurantsRepository,
        ILogger<CreatedMultipleRestaurantsRequirementHandler> logger,
        IUserContext userContext
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _userContext = userContext;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CreatedMultipleRestaurantsRequirement requirement
    )
    {
        var currentUser = _userContext.GetCurrentUser();

        _logger.LogInformation(
            "User: {Email} - handeling MinimumCreatedResaurantsRequirement",
            currentUser.Email
        );

        var resturants = await _restaurantsRepository.GetAllAsync();
        var userResturantsCreated = resturants.Count(r => r.OwnerId == currentUser!.Id);

        if (userResturantsCreated >= requirement.MinimumRestaurantsCreated)
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
