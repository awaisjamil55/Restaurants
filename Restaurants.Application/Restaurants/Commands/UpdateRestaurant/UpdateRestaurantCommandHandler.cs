using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<UpdateRestaurantCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateRestaurantCommandHandler(
        IRestaurantsRepository restaurantsRepository,
        ILogger<UpdateRestaurantCommandHandler> logger,
        IMapper mapper
    )
    {
        _restaurantsRepository = restaurantsRepository;
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

        _mapper.Map(request, restaurant);

        await _restaurantsRepository.SaveChanges();
    }
}
