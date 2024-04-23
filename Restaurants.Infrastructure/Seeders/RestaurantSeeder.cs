using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext restaurantsDbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (await restaurantsDbContext.Database.CanConnectAsync())
        {
            if (!restaurantsDbContext.Restaurants.Any())
            {
                restaurantsDbContext.Restaurants.AddRange(GetRestaurants());
                await restaurantsDbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        return new List<Restaurant>()
        {
            new Restaurant {
                Name="KFC",
                Category="Fast Food",
                Description="Fast food chain",
                ContactEmail="contactEmail@kfc.com",
                HasDelivery=true,
                Address = new()
                {
                    City="Lahore",
                    Street="Canal bank",
                    PostalCode="54840"
                },
                Dishes =
                [
                    new(){
                        Name="Zinger Burger",
                        Description="Zinger burger with fries",
                        Price=350
                    },
                    new(){
                        Name="Chicken Nuggets",
                        Description="Fries chicken nuggets (10 pcs)",
                        Price=250
                    }
                ]
            },
            new Restaurant {
                Name="Howdy",
                Category="Fast Food",
                Description="All beef and chicken varieties are available",
                ContactEmail="contactEmail@howdy.com",
                ContactNumber="+92123456789",
                HasDelivery=true,
                Address = new()
                {
                    City="Lahore",
                    Street="Canal bank",
                    PostalCode="54840"
                },
                Dishes = 
                [
                    new(){
                        Name="Dasher",
                        Description="Beef burger with fries",
                        Price=550
                    },
                    new(){
                        Name="Chicken Burger",
                        Description="Petty burger",
                        Price=300
                    }
                ]
            }
        };
    }
}
