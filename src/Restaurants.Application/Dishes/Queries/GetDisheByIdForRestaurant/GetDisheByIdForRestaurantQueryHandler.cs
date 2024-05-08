using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDisheByIdForRestaurant;

public class GetDisheByIdForRestaurantQueryHandler
    : IRequestHandler<GetDisheByIdForRestaurantQuery, DishDto>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<GetDisheByIdForRestaurantQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetDisheByIdForRestaurantQueryHandler(
        IRestaurantsRepository restaurantsRepository,
        ILogger<GetDisheByIdForRestaurantQueryHandler> logger,
        IMapper mapper
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<DishDto> Handle(
        GetDisheByIdForRestaurantQuery request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Fetching dish {DishId} for Restaurant {RestaurantId}",
            request.DishId,
            request.RestaurantId
        );

        var restaurant =
            await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new EntityNotFoundException(
                nameof(Restaurant),
                request.RestaurantId.ToString()
            );

        var dish =
            restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId)
            ?? throw new EntityNotFoundException(nameof(Dish), request.DishId.ToString());

        return _mapper.Map<DishDto>(dish);
    }
}
