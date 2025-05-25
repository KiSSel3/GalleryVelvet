using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.DAL.Repositories.Interfaces;
using GalleryVelvet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalleryVelvet.BLL.Services.Implementations;

public sealed class CartItemService(
    ICartItemRepository cartItemRepository,
    IProductRepository productRepository,
    ISizeRepository sizeRepository) : ICartItemService
{
    public async Task<IEnumerable<CartItemEntity>> GetCartItemsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await cartItemRepository
            .GetQueryable(
                filter: ci => ci.UserId == userId,
                includeBuilder: q => q
                    .Include(ci => ci.Size)
                    .Include(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                    .Include(ci => ci.Product)
                        .ThenInclude(p => p.Category)
            )
            .ToListAsync(cancellationToken);
    }

    public async Task<CartItemEntity?> AddToCartAsync(
        Guid userId,
        Guid productId,
        Guid sizeId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        var existing = await cartItemRepository.GetFirstOrDefaultAsync(
            x => x.UserId == userId && x.ProductId == productId && x.SizeId == sizeId,
            cancellationToken: cancellationToken
        );

        if (existing != null)
        {
            existing.Quantity += quantity;

            if (existing.Quantity <= 0)
            {
                await cartItemRepository.HardDeleteAsync(existing, cancellationToken);
            }
            else
            {
                await cartItemRepository.UpdateAsync(existing, cancellationToken);
            }
            
            return existing;
        }

        var product = await productRepository
            .GetByIdAsync(productId, cancellationToken: cancellationToken);

        if (product is null)
        {
            throw new Exception();
        }
        
        var size = await sizeRepository
            .GetByIdAsync(sizeId, cancellationToken: cancellationToken);
        
        if (size is null)
        {
            throw new Exception();
        }

        var newItem = new CartItemEntity
        {
            UserId = userId,
            ProductId = productId,
            SizeId = sizeId,
            Quantity = quantity
        };

        await cartItemRepository.CreateAsync(newItem, cancellationToken);
        
        return newItem;
    }

    public async Task<bool> RemoveFromCartAsync(
        Guid userId,
        Guid cartItemId,
        CancellationToken cancellationToken = default)
    {
        var item = await cartItemRepository.GetByIdAsync(cartItemId, cancellationToken: cancellationToken);
        
        if (item is null || item.UserId != userId)
        {
            throw new Exception();
        }

        await cartItemRepository.HardDeleteAsync(item, cancellationToken);
        
        return true;
    }

    public Task ClearCartAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return cartItemRepository.DeleteByUserIdAsync(userId, cancellationToken);
    }
}