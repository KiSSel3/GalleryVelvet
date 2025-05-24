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

    public virtual Task<TEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = _dbSet.Where(e => e.Id == id);

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = _dbSet.Where(filter);

        foreach (var include in includeProperties)
        {
            query = query.Include(include);
        }

        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual IQueryable<TEntity> GetQueryable(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = _dbSet.AsNoTracking();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        return query;
    }
}