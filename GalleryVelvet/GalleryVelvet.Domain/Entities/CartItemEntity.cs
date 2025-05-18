using GalleryVelvet.Domain.Common;

namespace GalleryVelvet.Domain.Entities;

public sealed class CartItemEntity : BaseEntity
{
    public int Quantity { get; set; }
    
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
    
    public Guid SizeId { get; set; }
    public SizeEntity Size { get; set; } = null!;
    
    public ICollection<CartItemEntity> CartItems { get; set; } = [];
    public ICollection<OrderItemEntity> OrderItems { get; set; } = [];
}