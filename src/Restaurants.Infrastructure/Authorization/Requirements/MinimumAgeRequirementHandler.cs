using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    private readonly ILogger<MinimumAgeRequirementHandler> _logger;
    private readonly IUserContext _userContext;

    public MinimumAgeRequirementHandler(
        ILogger<MinimumAgeRequirementHandler> logger,
        IUserContext userContext
    )
    {
        _logger = logger;
        _userContext = userContext;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement
    )
    {
        var currentUser = _userContext.GetCurrentUser();

        _logger.LogInformation(
            "User: {Email}, DOB {DOB} - Handeling MinimumAgeRequirement",
            currentUser.Email,
            currentUser.DateOfBirth
        );

        if (currentUser.DateOfBirth == null)
        {
            _logger.LogWarning("User date of birth is null");
            context.Fail();
            return Task.CompletedTask;
        }

        if (
            currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge)
            <= DateOnly.FromDateTime(DateTime.Today)
        )
        {
            _logger.LogInformation("Minimum Age authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}
