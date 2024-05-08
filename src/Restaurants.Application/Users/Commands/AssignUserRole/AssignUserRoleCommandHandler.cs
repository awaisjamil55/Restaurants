using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler : IRequestHandler<AssignUserRoleCommand>
{
    private readonly ILogger<AssignUserRoleCommandHandler> _logger;

    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AssignUserRoleCommandHandler(
        ILogger<AssignUserRoleCommandHandler> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _logger = logger;

        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Assigning user role {@Request}", request);

        var user =
            await _userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new EntityNotFoundException(nameof(User), request.UserEmail);

        var role =
            await _roleManager.FindByNameAsync(request.RoleName)
            ?? throw new EntityNotFoundException(nameof(IdentityRole), request.RoleName);

        await _userManager.AddToRoleAsync(user, role.Name!);
    }
}
