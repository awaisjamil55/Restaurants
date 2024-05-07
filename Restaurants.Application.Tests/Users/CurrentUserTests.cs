using FluentAssertions;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Users.Tests;

public class CurrentUserTests
{
    [Theory]
    [InlineData(UserRoles.User)]
    [InlineData(UserRoles.Owner)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        // arrange
        var currentUser = new CurrentUser(
            "1",
            "test@test.com",
            [UserRoles.User, UserRoles.Owner],
            null,
            null,
            null
        );

        // act
        var isInRole = currentUser.IsInRole(roleName);

        // assert
        isInRole.Should().BeTrue();
    }

    [Fact]
    public void IsInRole_WithNoMatchingRoleCase_ShouldReturnFalse()
    {
        // arrange
        var currentUser = new CurrentUser(
            "1",
            "test@test.com",
            [UserRoles.User, UserRoles.Owner],
            null,
            null,
            null
        );

        // act
        var isInRole = currentUser.IsInRole(UserRoles.Admin);

        // assert
        isInRole.Should().BeFalse();
    }
}
