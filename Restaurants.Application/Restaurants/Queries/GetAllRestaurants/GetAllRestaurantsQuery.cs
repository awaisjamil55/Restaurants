using MediatR;
using Restaurants.Application.Pagination;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery : IRequest<PagedResult<RestaurantDto>>
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
