using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteDishesForRestaurant;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetDisheByIdForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/dishes")]
[ApiController]
public class DishesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DishesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDish(
        int restaurantId,
        [FromBody] CreateDishCommand command
    )
    {
        command.RestaurantId = restaurantId;

        var dishId = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetByIdForRestaurant), new { restaurantId, dishId }, null);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAllForRestaurant(int restaurantId) =>
        Ok(await _mediator.Send(new GetDishesForRestaurantQuery(restaurantId)));

    [HttpGet]
    [Route("{dishId}")]
    public async Task<ActionResult<DishDto>> GetByIdForRestaurant(int restaurantId, int dishId) =>
        Ok(await _mediator.Send(new GetDisheByIdForRestaurantQuery(restaurantId, dishId)));

    [HttpDelete]
    public async Task<IActionResult> DeleteDishesForRestaurant(int restaurantId)
    {
        await _mediator.Send(new DeleteDishesForRestaurantCommand(restaurantId));

        return NoContent();
    }
}
