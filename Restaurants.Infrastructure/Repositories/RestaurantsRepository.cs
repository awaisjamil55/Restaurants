using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository : IRestaurantsRepository
{
    private readonly RestaurantsDbContext _dbContext;

    public RestaurantsRepository(
        RestaurantsDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync() =>
        await _dbContext.Restaurants.ToListAsync();

    public async Task<Restaurant?> GetByIdAsync(int id) =>
        await _dbContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id);

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

    public Task SaveChanges() =>
        _dbContext.SaveChangesAsync();
}
