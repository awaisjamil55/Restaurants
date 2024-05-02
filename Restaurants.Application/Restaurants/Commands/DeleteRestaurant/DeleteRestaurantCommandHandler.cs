using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IRestaurantAuthorizationService _restaurantAuthorizationService;
    private readonly ILogger<DeleteRestaurantCommandHandler> _logger;
    private readonly IMapper _mapper;

    public DeleteRestaurantCommandHandler(
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService,
        ILogger<DeleteRestaurantCommandHandler> logger,
        IMapper mapper
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _restaurantAuthorizationService = restaurantAuthorizationService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting a restaurant: {RestaurantId}", request.Id);

        var restaurant =
            await _restaurantsRepository.GetByIdAsync(request.Id)
            ?? throw new EntityNotFoundException(nameof(Restaurant), request.Id.ToString());

        if (!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbidException();

        await _restaurantsRepository.Delete(restaurant);
    }
}
