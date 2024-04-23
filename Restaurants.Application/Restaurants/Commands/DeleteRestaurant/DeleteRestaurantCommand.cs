using MediatR;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommand : IRequest<bool>
{
    public DeleteRestaurantCommand(int id)
    {
        Id = id;
    }

    public int Id { get; }
}
