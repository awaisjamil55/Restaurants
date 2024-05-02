using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IRestaurantAuthorizationService _restaurantAuthorizationService;
    private readonly ILogger<UpdateRestaurantCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateRestaurantCommandHandler(
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService,
        ILogger<UpdateRestaurantCommandHandler> logger,
        IMapper mapper
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _restaurantAuthorizationService = restaurantAuthorizationService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Updating a restaurant {RestaurantId}: {@UpdatedRestaurant}",
            request.Id,
            request
        );

        var restaurant =
            await _restaurantsRepository.GetByIdAsync(request.Id)
            ?? throw new EntityNotFoundException(nameof(Restaurant), request.Id.ToString());

        if (!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException();

        _mapper.Map(request, restaurant);

        await _restaurantsRepository.SaveChanges();
    }
}
