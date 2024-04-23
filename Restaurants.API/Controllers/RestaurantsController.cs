using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RestaurantsController(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    //public async Task<IActionResult> Get([FromRoute]int id)
    public async Task<IActionResult> Get(int id)
    {
        var restaurant = await _mediator.Send(new GetRestaurantByIdQuery(id));

        if (restaurant == null)
        {
            return NotFound();
        }

        return Ok(restaurant);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllRestaurantsQuery()));
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand command)
    {
        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> UpdateRestaurant(int id, [FromBody] UpdateRestaurantCommand command)
    {
        command.Id = id;
        var i = await _mediator.Send(command);

        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteRestaurant(int id)
    {
        if (await _mediator.Send(new DeleteRestaurantCommand(id)))
        {
            return NoContent();
        }

        return NotFound();
    }
}
