using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;

public class UnAssignUserRoleCommandHandler : IRequestHandler<UnassignUserRoleCommand>
{
    private readonly ILogger<UnAssignUserRoleCommandHandler> _logger;

    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UnAssignUserRoleCommandHandler(
        ILogger<UnAssignUserRoleCommandHandler> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _logger = logger;

        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UnAssigning user role {@Request}", request);

        var user =
            await _userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new EntityNotFoundException(nameof(User), request.UserEmail);

        var role =
            await _roleManager.FindByNameAsync(request.RoleName)
            ?? throw new EntityNotFoundException(nameof(IdentityRole), request.RoleName);

        await _userManager.RemoveFromRoleAsync(user, role.Name!);
    }
}
