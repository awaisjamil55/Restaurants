using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext restaurantsDbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (restaurantsDbContext.Database.GetPendingMigrations().Any())
        {
            await restaurantsDbContext.Database.MigrateAsync();
        }

        if (await restaurantsDbContext.Database.CanConnectAsync())
        {
            if (!restaurantsDbContext.Restaurants.Any())
            {
                restaurantsDbContext.Restaurants.AddRange(GetRestaurants());
                await restaurantsDbContext.SaveChangesAsync();
            }

            if (!restaurantsDbContext.Roles.Any())
            {
                restaurantsDbContext.Roles.AddRange(GetRoles());
                await restaurantsDbContext.SaveChangesAsync();
            }
        }
    }

    public IEnumerable<IdentityRole> GetRoles()
    {
        var roles = new List<IdentityRole>()
        {
            new(UserRoles.User) { NormalizedName = UserRoles.User.ToUpper() },
            new(UserRoles.Owner) { NormalizedName = UserRoles.Owner.ToUpper() },
            new(UserRoles.Admin) { NormalizedName = UserRoles.Admin.ToUpper() }
        };

        return roles;
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        var owner = new User() { Email = "seed-user@test.com" };

        return new List<Restaurant>()
        {
            new Restaurant
            {
                Name = "KFC",
                Category = "Fast Food",
                Description = "Fast food chain",
                ContactEmail = "contactEmail@kfc.com",
                HasDelivery = true,
                Address = new()
                {
                    City = "Lahore",
                    Street = "Canal bank",
                    PostalCode = "54840"
                },
                Dishes =
                [
                    new()
                    {
                        Name = "Zinger Burger",
                        Description = "Zinger burger with fries",
                        Price = 350
                    },
                    new()
                    {
                        Name = "Chicken Nuggets",
                        Description = "Fries chicken nuggets (10 pcs)",
                        Price = 250
                    }
                ],
                Owner = owner
            },
            new Restaurant
            {
                Name = "Howdy",
                Category = "Fast Food",
                Description = "All beef and chicken varieties are available",
                ContactEmail = "contactEmail@howdy.com",
                ContactNumber = "+92123456789",
                HasDelivery = true,
                Address = new()
                {
                    City = "Lahore",
                    Street = "Canal bank",
                    PostalCode = "54840"
                },
                Dishes =
                [
                    new()
                    {
                        Name = "Dasher",
                        Description = "Beef burger with fries",
                        Price = 550
                    },
                    new()
                    {
                        Name = "Chicken Burger",
                        Description = "Petty burger",
                        Price = 300
                    }
                ],
                Owner = owner
            }
        };
    }
}
