using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MultipleOwnedRestaurantsRequirement(int minimumOwnedRestaurants)
    : IAuthorizationRequirement
{
    public int MinimumOwnedRestaurants { get; set; } = minimumOwnedRestaurants;
}
