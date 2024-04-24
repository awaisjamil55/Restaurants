namespace Restaurants.Application.Dishes.Dtos;

public class DishDto
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Price { get; set; }
    public int? KiloCalories { get; set; }
}
