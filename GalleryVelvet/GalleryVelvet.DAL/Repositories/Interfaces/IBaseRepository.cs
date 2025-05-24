using System.Linq.Expressions;
using GalleryVelvet.Domain.Common;

namespace GalleryVelvet.DAL.Repositories.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task HardDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(
        Guid id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder = null,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder = null,
        CancellationToken cancellationToken = default);

    IQueryable<TEntity> GetQueryable(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder = null);
}