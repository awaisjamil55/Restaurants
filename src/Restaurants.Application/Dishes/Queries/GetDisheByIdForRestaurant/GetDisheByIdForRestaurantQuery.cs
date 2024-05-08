using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDisheByIdForRestaurant;

public class GetDisheByIdForRestaurantQuery : IRequest<DishDto>
{
    public GetDisheByIdForRestaurantQuery(int restaurantId, int dishId)
    {
        RestaurantId = restaurantId;
        DishId = dishId;
    }

    public int RestaurantId { get; set; }
    public int DishId { get; set; }
}
