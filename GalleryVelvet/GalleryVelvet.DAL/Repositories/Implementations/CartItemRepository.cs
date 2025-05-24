using GalleryVelvet.DAL.Infrastructure;
using GalleryVelvet.DAL.Repositories.Interfaces;
using GalleryVelvet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalleryVelvet.DAL.Repositories.Implementations;

public sealed class CartItemRepository(AppDbContext dbContext) 
    : BaseRepository<CartItemEntity>(dbContext), ICartItemRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public Task DeleteByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _dbContext.CartItems
            .Where(ci => ci.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}