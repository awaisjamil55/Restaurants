namespace Restaurants.Infrastructure.Authorization;

public static class Policies
{
    public const string HavePassport = "HavePassport";
    public const string MinimumAge18 = "MinimumAge18";
    public const string Minimum2restaurantsOwned = "Minimum2restaurantsOwned";
}

public static class AppClaimTypes
{
    public const string Nationality = "Nationality";
    public const string DateOfBirth = "DateOfBirth";
    public const string HavePassport = "HavePassport";
}
