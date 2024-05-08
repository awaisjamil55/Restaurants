using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDishesForRestaurant;

public class DeleteDishesForRestaurantCommand : IRequest
{
    public DeleteDishesForRestaurantCommand(int restaurantId)
    {
        RestaurantId = restaurantId;
    }

    public int RestaurantId { get; }
}
