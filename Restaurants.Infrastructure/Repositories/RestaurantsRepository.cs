using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository : IRestaurantsRepository
{
    private readonly RestaurantsDbContext _dbContext;

    public RestaurantsRepository(RestaurantsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync(int offset, int limit) =>
        await _dbContext.Restaurants.Skip(offset).Take(limit).ToListAsync();

    public async Task<int> CountAsync(int offset, int limit) =>
        await _dbContext.Restaurants.CountAsync();

    public async Task<IEnumerable<Restaurant>> GetOwnedRestaurantsByUserIdAsync(string userId) =>
        await _dbContext.Restaurants.Where(r => r.OwnerId == userId).ToListAsync();

    public async Task<int> GetOwnedRestaurantsCountByUserIdAsync(string userId) =>
        await _dbContext.Restaurants.CountAsync(r => r.OwnerId == userId);

    public async Task<IEnumerable<Restaurant>> SearchAsync(
        string? searchTerm,
        int offset,
        int limit
    ) => await GetSearchQuery(searchTerm, offset, limit).ToListAsync();

    public async Task<int> SearchCountAsync(string? searchTerm, int offset, int limit) =>
        await GetSearchQuery(searchTerm, offset, limit).CountAsync();

    public async Task<Restaurant?> GetByIdAsync(int id) =>
        await _dbContext.Restaurants.Include(x => x.Dishes).FirstOrDefaultAsync(x => x.Id == id);

    public async Task<int> Create(Restaurant entity)
    {
        _dbContext.Restaurants.Add(entity);
        await _dbContext.SaveChangesAsync();

        return entity.Id;
    }

    public async Task Delete(Restaurant entity)
    {
        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public Task SaveChanges() => _dbContext.SaveChangesAsync();

    private IQueryable<Restaurant> GetSearchQuery(string? searchTerm, int offset, int limit)
    {
        var searchTermLower = searchTerm?.ToLower();

        return _dbContext
            .Restaurants.Where(r =>
                searchTermLower == null
                || (
                    r.Name.ToLower().Contains(searchTermLower)
                    || r.Description.ToLower().Contains(searchTermLower)
                )
            )
            .Skip(offset)
            .Take(limit);
    }
}
