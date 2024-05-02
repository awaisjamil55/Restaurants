using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, int>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<CreateRestaurantCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public CreateRestaurantCommandHandler(
        IRestaurantsRepository restaurantsRepository,
        ILogger<CreateRestaurantCommandHandler> logger,
        IMapper mapper,
        IUserContext userContext
    )
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<int> Handle(
        CreateRestaurantCommand request,
        CancellationToken cancellationToken
    )
    {
        var currentUser = _userContext.GetCurrentUser();

        _logger.LogInformation(
            "{UserEmail} [{UserId}] is creating a new restaurant {@Restaurant}",
            currentUser.Email,
            currentUser.Id,
            request
        );

        var restaurant = _mapper.Map<Restaurant>(request);
        restaurant.OwnerId = currentUser.Id;

        return await _restaurantsRepository.Create(restaurant);
    }
}
