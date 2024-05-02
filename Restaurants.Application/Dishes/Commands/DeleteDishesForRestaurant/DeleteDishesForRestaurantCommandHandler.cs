using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishesForRestaurant;

public class DeleteDishesForRestaurantCommandHandler
    : IRequestHandler<DeleteDishesForRestaurantCommand>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IDishesRepository _dishesRepository;
    private readonly IRestaurantAuthorizationService _restaurantAuthorizationService;

    private readonly ILogger<DeleteDishesForRestaurantCommandHandler> _logger;
    private readonly IMapper _mapper;

    public DeleteDishesForRestaurantCommandHandler(
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService,
        ILogger<DeleteDishesForRestaurantCommandHandler> logger,
        IMapper mapper
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _dishesRepository = dishesRepository;
        _restaurantAuthorizationService = restaurantAuthorizationService;

        _logger = logger;
        _mapper = mapper;
    }

    public async Task Handle(
        DeleteDishesForRestaurantCommand request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Deleting all dishes for restaurant {RestaurantId}",
            request.RestaurantId
        );

        var restaurant =
            await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new EntityNotFoundException(
                nameof(Restaurant),
                request.RestaurantId.ToString()
            );

        if (!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException();

        await _dishesRepository.Delete(restaurant.Dishes);
    }
}
