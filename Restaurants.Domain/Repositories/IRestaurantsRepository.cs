using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync(int offset, int limit);
    Task<int> CountAsync(int offset, int limit);
    Task<IEnumerable<Restaurant>> GetOwnedRestaurantsByUserIdAsync(string userId);
    Task<int> GetOwnedRestaurantsCountByUserIdAsync(string userId);
    Task<IEnumerable<Restaurant>> SearchAsync(string? searchTerm, int offset, int limit);
    Task<Restaurant?> GetByIdAsync(int id);
    Task<int> Create(Restaurant entity);
    Task Delete(Restaurant entity);
    Task SaveChanges();
}
