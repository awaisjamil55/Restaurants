using AutoMapper;
using Restaurants.Application.Users.UpdateUserDetails;
using Restaurants.Domain.Entities.Identity;

namespace Restaurants.Application.Users;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<UpdateUserDetailsCommand, User>();
    }
}
