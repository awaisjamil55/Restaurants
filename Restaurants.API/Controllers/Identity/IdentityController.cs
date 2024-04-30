using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.UnassignUserRole;
using Restaurants.Application.Users.UpdateUserDetails;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers.Identity;

[ApiController]
[Route("api/identity")]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;

    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPatch("user")]
    [Authorize]
    public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPatch("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> UnassignUserRole(UnassignUserRoleCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }
}
