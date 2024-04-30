using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Entities.Identity;

namespace Restaurants.Infrastructure.Authorization;

public class RestaurantsUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole>
{
    public RestaurantsUserClaimsPrincipalFactory(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options
    )
        : base(userManager, roleManager, options) { }

    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var id = await GenerateClaimsAsync(user);

        if (user.Nationality != null)
        {
            id.AddClaim(new Claim(AppClaimTypes.Nationality, user.Nationality));
        }

        if (user.DateOfBirth != null)
        {
            id.AddClaim(
                new Claim(AppClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToString("yyyy-MM-dd"))
            );
        }

        if (user.HavePassport != null)
        {
            id.AddClaim(
                new Claim(AppClaimTypes.HavePassport, user.HavePassport == true ? "Yes" : "No")
            );
        }

        return new ClaimsPrincipal(id);
    }
}
