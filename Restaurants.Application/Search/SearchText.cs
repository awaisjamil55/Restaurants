using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Pagination;

namespace Restaurants.Application.Search;

public class SearchText : PaginationSettings
{
    [FromQuery]
    public string? SearchTerm { get; set; }
}
