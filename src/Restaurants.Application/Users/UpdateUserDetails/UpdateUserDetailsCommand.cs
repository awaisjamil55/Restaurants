using MediatR;

namespace Restaurants.Application.Users.UpdateUserDetails;

public class UpdateUserDetailsCommand : IRequest
{
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public bool? HavePassport { get; set; }
}
