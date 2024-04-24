using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<DeleteRestaurantCommandHandler> _logger;
    private readonly IMapper _mapper;

    public DeleteRestaurantCommandHandler(
        IRestaurantsRepository restaurantsRepository,
        ILogger<DeleteRestaurantCommandHandler> logger,
        IMapper mapper
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting a restaurant: {RestaurantId}", request.Id);

        var restaurant =
            await _restaurantsRepository.GetByIdAsync(request.Id)
            ?? throw new EntityNotFoundException(nameof(Restaurant), request.Id.ToString());

        await _restaurantsRepository.Delete(restaurant);
    }
}
