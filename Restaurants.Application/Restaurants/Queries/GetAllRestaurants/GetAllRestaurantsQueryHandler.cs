using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler
    : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<GetAllRestaurantsQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetAllRestaurantsQueryHandler(
        IRestaurantsRepository restaurantsRepository,
        ILogger<GetAllRestaurantsQueryHandler> logger,
        IMapper mapper
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RestaurantDto>> Handle(
        GetAllRestaurantsQuery request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("Getting all restaurants");

        var restaurants = await _restaurantsRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
    }
}
