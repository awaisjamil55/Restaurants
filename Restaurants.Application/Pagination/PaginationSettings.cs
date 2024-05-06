using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Restaurants.Application.Pagination;

public class PaginationSettings
{
    [FromQuery]
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int Offset { get; set; } = 0;

    [FromQuery]
    [DefaultValue(20)]
    [Range(1, int.MaxValue)]
    public int Limit { get; set; } = 20;
}
