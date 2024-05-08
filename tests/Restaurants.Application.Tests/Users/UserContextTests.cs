using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Users.Tests;

public class UserContextTests
{
    [Fact()]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        // arrange
        var dob = new DateOnly(1996, 2, 3);

        var httpContextAccessor = new Mock<IHttpContextAccessor>();

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@test.com"),
            new(ClaimTypes.Role, UserRoles.User),
            new(ClaimTypes.Role, UserRoles.Admin),
            new("Nationality", "Pakistani"),
            new("DateOfBirth", dob.ToString("yyyy-MM-dd"))
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        httpContextAccessor
            .Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext() { User = user });
        var userContext = new UserContext(httpContextAccessor.Object);

        // act
        var currentUser = userContext.GetCurrentUser();

        // assert

        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.User, UserRoles.Admin);
        currentUser.Nationality.Should().Be("Pakistani");
        currentUser.DateOfBirth.Should().Be(dob);
    }

    [Fact()]
    public void GetCurrentUser_WithUserContextNotPresent_ThrowInvalidOperationException()
    {
        // arrange
        var httpContextAccessor = new Mock<IHttpContextAccessor>();

        httpContextAccessor.Setup(x => x.HttpContext).Returns((HttpContext)null);
        var userContext = new UserContext(httpContextAccessor.Object);

        // act
        var action = () => userContext.GetCurrentUser();

        // assert

        action
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("User context doesn't exist");
    }
}
