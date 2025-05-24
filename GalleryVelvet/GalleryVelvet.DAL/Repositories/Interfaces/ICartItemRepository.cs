using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.DAL.Repositories.Interfaces;

public interface ICartItemRepository : IBaseRepository<CartItemEntity>
{
    Task DeleteByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}