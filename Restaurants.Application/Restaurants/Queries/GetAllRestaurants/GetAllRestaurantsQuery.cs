using MediatR;
using Restaurants.Application.Pagination;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery : PaginationSettings, IRequest<IEnumerable<RestaurantDto>> { }
