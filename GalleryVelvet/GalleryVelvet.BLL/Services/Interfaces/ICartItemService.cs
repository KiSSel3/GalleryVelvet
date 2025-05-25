using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.BLL.Services.Interfaces;

public interface ICartItemService
{
    Task<IEnumerable<CartItemEntity>> GetCartItemsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
    
    Task<CartItemEntity?> AddToCartAsync(
        Guid userId,
        Guid productId,
        Guid sizeId,
        int quantity,
        CancellationToken cancellationToken = default);
    
    Task<bool> RemoveFromCartAsync(
        Guid userId,
        Guid cartItemId,
        CancellationToken cancellationToken = default);

    Task ClearCartAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}