using MediatR;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Search;

namespace Restaurants.Application.Restaurants.Queries.Search;

public class SearchRestaurantsQuery : SearchText, IRequest<IEnumerable<RestaurantDto>> { }
