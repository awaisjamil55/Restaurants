﻿using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<Restaurant>> GetAllAsync() =>
        await _dbContext.Restaurants.ToListAsync();

    public async Task<(IEnumerable<Restaurant>, int)> SearchAsync(
        string? searchTerm,
        int pageNumber,
        int pageSize
    )
    {
        var searchTermLower = searchTerm?.ToLower();

        var query = _dbContext.Restaurants.Where(r =>
            searchTermLower == null
            || (
                r.Name.ToLower().Contains(searchTermLower)
                || r.Description.ToLower().Contains(searchTermLower)
            )
        );

        return (
            await query.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync(),
            await query.CountAsync()
        );
    }

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
}
