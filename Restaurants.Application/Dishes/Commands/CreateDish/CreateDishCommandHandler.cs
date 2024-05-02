using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler : IRequestHandler<CreateDishCommand, int>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IDishesRepository _dishesRepository;
    private readonly IRestaurantAuthorizationService _restaurantAuthorizationService;

    private readonly ILogger<CreateDishCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateDishCommandHandler(
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService,
        ILogger<CreateDishCommandHandler> logger,
        IMapper mapper
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _dishesRepository = dishesRepository;
        _restaurantAuthorizationService = restaurantAuthorizationService;

        _logger = logger;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new dish: {@DishRequest}", request);

        var restaurant =
            await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new EntityNotFoundException(
                nameof(Restaurant),
                request.RestaurantId.ToString()
            );

        if (!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException();

        return await _dishesRepository.Create(_mapper.Map<Dish>(request));
    }
}
