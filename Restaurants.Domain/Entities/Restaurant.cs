using Restaurants.Domain.Entities.Identity;

namespace Restaurants.Domain.Entities;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool HasDelivery { get; set; }

    public string? ContactEmail { get; set; }
    public string? ContactNumber { get; set; }

    public Address? Address { get; set; }
    public IList<Dish> Dishes { get; set; } = new List<Dish>();
    public User Owner { get; set; }
    public string OwnerId { get; set; } = string.Empty;
}
