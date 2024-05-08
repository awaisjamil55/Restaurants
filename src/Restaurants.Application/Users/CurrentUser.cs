namespace Restaurants.Application.Users;

public record CurrentUser(
    string Id,
    string Email,
    IEnumerable<string> Roles,
    string? Nationality,
    DateOnly? DateOfBirth,
    bool? HavePassport
)
{
    public bool IsInRole(string Role) => Roles.Contains(Role);
}
