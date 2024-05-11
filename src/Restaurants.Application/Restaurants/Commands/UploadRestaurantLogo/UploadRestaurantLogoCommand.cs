using MediatR;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommand : IRequest
{
    public int RestauranId { get; set; }
    public string FileName { get; set; } = default!;
    public Stream File { get; set; } = default!;
}
