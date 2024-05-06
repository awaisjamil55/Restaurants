using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Application.Restaurants.Queries.Search;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
[Authorize]
public class RestaurantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RestaurantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<RestaurantDto?>> Get(int id) =>
        Ok(await _mediator.Send(new GetRestaurantByIdQuery(id)));

    [HttpGet]
    [AllowAnonymous]
    //[Authorize(Policy = Policies.Minimum2ResturantsOwned)]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll(
        [FromQuery] GetAllRestaurantsQuery query
    ) => Ok(await _mediator.Send(query));

    [HttpGet]
    [Route("search")]
    [AllowAnonymous]
    //[Authorize(Policy = Policies.Minimum2ResturantsOwned)]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> Search(
        [FromQuery] SearchResturantsQuery query
    ) => Ok(await _mediator.Send(query));

    [HttpPost]
    [Authorize(Roles = UserRoles.Owner)]
    [Authorize(Policy = Policies.HavePassport)]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand command)
    {
        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPatch]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRestaurant(
        int id,
        [FromBody] UpdateRestaurantCommand command
    )
    {
        command.Id = id;
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Policy = Policies.MinimumAge18)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant(int id)
    {
        await _mediator.Send(new DeleteRestaurantCommand(id));

        return NoContent();
    }
}
