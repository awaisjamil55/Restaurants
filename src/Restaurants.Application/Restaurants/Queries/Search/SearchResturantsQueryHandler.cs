using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.Search;

public class SearchResturantsQueryHandler
    : IRequestHandler<SearchResturantsQuery, IEnumerable<RestaurantDto>>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<SearchResturantsQueryHandler> _logger;
    private readonly IMapper _mapper;

    public SearchResturantsQueryHandler(
        IRestaurantsRepository restaurantsRepository,
        ILogger<SearchResturantsQueryHandler> logger,
        IMapper mapper
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RestaurantDto>> Handle(
        SearchResturantsQuery request,
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
