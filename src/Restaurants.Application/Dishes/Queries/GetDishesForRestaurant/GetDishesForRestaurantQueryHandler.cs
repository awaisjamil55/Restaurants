using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;

public class GetDishesForRestaurantQueryHandler
    : IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDto>>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IDishesRepository _dishesRepository;

    private readonly ILogger<GetDishesForRestaurantQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetDishesForRestaurantQueryHandler(
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository,
        ILogger<GetDishesForRestaurantQueryHandler> logger,
        IMapper mapper
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _dishesRepository = dishesRepository;

        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DishDto>> Handle(
        GetDishesForRestaurantQuery request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Fetching dishes for restaurant {RestaurantId}",
            request.RestaurantId
        );

        var restaurant =
            await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new EntityNotFoundException(
                nameof(Restaurant),
                request.RestaurantId.ToString()
            );

        return _mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);
    }
}
