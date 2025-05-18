using Microsoft.EntityFrameworkCore;

namespace GalleryVelvet.BLL.RequestFeatures;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; set; }

    public PagedList() : base()
    {
        MetaData = new MetaData
        {
            TotalCount = 0,
            PageSize = 0,
            CurrentPage = 0,
            TotalPages = 0
        };
    }
    
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData
        {
            TotalCount = count,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };

        AddRange(items);
    }

    public static async Task<PagedList<T>> ToPagedListAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken: cancellationToken);
        
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}