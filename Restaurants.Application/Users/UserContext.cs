using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Restaurants.Application.Users;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUser? GetCurrentUser()
    {
        var user =
            _httpContextAccessor?.HttpContext?.User
            ?? throw new InvalidOperationException("User context doesn't exist");

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return null;
        }

        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        var nationality = user.FindFirst(c => c.Type == "Nationality")?.Value;
        var dateOfBirthString = user.FindFirst(c => c.Type == "DateOfBirth")?.Value;
        var havePassportString = user.FindFirst(c => c.Type == "HavePassport")?.Value;

        var dateOfBirth =
            dateOfBirthString == null
                ? (DateOnly?)null
                : DateOnly.ParseExact(dateOfBirthString, "yyyy-MM-dd");
        var havePassport = havePassportString == null ? (bool?)null : havePassportString == "True";

        return new CurrentUser(userId, email, roles, nationality, dateOfBirth, havePassport);
    }
}
