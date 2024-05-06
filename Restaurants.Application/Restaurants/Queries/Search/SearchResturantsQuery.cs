using MediatR;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Search;

namespace Restaurants.Application.Restaurants.Queries.Search;

public class SearchResturantsQuery : SearchText, IRequest<IEnumerable<RestaurantDto>> { }
