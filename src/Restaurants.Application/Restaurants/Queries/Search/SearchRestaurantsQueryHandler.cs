using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.Search;

public class SearchRestaurantsQueryHandler
    : IRequestHandler<SearchRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<SearchRestaurantsQueryHandler> _logger;
    private readonly IMapper _mapper;

    public SearchRestaurantsQueryHandler(
        IRestaurantsRepository restaurantsRepository,
        ILogger<SearchRestaurantsQueryHandler> logger,
        IMapper mapper
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RestaurantDto>> Handle(
        SearchRestaurantsQuery request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Fetching {Limit} restaurants from offset {Offset}. Search Term: {SearchTerm}",
            request.Limit,
            request.Offset,
            request.SearchTerm
        );

        var restaurants = await _restaurantsRepository.SearchAsync(
            request.SearchTerm,
            request.Offset,
            request.Limit
        );

        return _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
    }
}
