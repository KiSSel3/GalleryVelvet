using System.Linq.Expressions;
using GalleryVelvet.DAL.Infrastructure;
using GalleryVelvet.DAL.Repositories.Interfaces;
using GalleryVelvet.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace GalleryVelvet.DAL.Repositories.Implementations;

public class BaseRepository<TEntity>(AppDbContext dbContext) : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public virtual async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        
        _dbSet.Update(entity);
        
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual Task HardDeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<TEntity?> GetByIdAsync(
        Guid id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(e => e.Id == id);

        if (includeBuilder is not null)
        {
            query = includeBuilder(query);
        }

        return query.FirstOrDefaultAsync(cancellationToken);
    }
    
    public Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(filter);

        if (includeBuilder is not null)
        {
            query = includeBuilder(query);
        }

        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public IQueryable<TEntity> GetQueryable(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder = null)
    {
        var query = _dbSet.AsNoTracking();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        if (includeBuilder is not null)
        {
            query = includeBuilder(query);
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        return query;
    }
}