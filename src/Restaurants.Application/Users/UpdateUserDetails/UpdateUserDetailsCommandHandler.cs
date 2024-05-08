using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.UpdateUserDetails;

public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailsCommand>
{
    private readonly ILogger<UpdateUserDetailsCommandHandler> _logger;
    private readonly IMapper _mapper;

    private readonly IUserContext _userContext;
    private readonly IUserStore<User> _userStore;

    public UpdateUserDetailsCommandHandler(
        ILogger<UpdateUserDetailsCommandHandler> logger,
        IMapper mapper,
        IUserContext userContext,
        IUserStore<User> userStore
    )
    {
        _logger = logger;
        _mapper = mapper;

        _userContext = userContext;
        _userStore = userStore;
    }

    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = _userContext.GetCurrentUser();

        _logger.LogInformation("Updating user {UserId} with {@Request}", user!.Id, request);

        var dbUser =
            await _userStore.FindByIdAsync(user!.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(User), user!.Id);

        await _userStore.UpdateAsync(_mapper.Map(request, dbUser), cancellationToken);
    }
}
