namespace Restaurants.Application.Pagination;

// Not using aywhere, if paginatin is not handeling at front-end
//Create new endpoint which returns object for pagination
public class PagedResult<TEntity>
{
    public PagedResult(IEnumerable<TEntity> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        ItemsFrom = pageSize * (pageNumber - 1) + 1;
        ItemsTo = ItemsFrom + pageSize - 1;
    }

    public IEnumerable<TEntity> Items { get; set; }
    public int TotalPages { get; set; }
    public int TotalItemsCount { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
}
