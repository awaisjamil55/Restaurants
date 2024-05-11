using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommandHandler : IRequestHandler<UploadRestaurantLogoCommand>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IRestaurantAuthorizationService _restaurantAuthorizationService;
    private readonly ILogger<UploadRestaurantLogoCommandHandler> _logger;
    private readonly IBlobStorageService _blobStorageService;

    public UploadRestaurantLogoCommandHandler(
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService,
        ILogger<UploadRestaurantLogoCommandHandler> logger,
        IBlobStorageService blobStorageService
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _restaurantAuthorizationService = restaurantAuthorizationService;
        _logger = logger;
        _blobStorageService = blobStorageService;
    }

    public async Task Handle(
        UploadRestaurantLogoCommand request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Updloading a logo for restaurant {RestaurantId}",
            request.RestauranId
        );

        var restaurant =
            await _restaurantsRepository.GetByIdAsync(request.RestauranId)
            ?? throw new EntityNotFoundException(
                nameof(Restaurant),
                request.RestauranId.ToString()
            );

        if (!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException();

        var logoUrl = await _blobStorageService.UploadToBlobAsync(request.File, request.FileName);
        restaurant.LogoUrl = logoUrl;

        await _restaurantsRepository.SaveChanges();
    }
}
