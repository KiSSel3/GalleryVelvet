using GalleryVelvet.Domain.Common;

namespace GalleryVelvet.Domain.Entities;

public sealed class OrderItemEntity : BaseEntity
{
    public int Quantity { get; set; }
    
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
    
    public Guid SizeId { get; set; }
    public SizeEntity Size { get; set; } = null!;
    
    public Guid OrderId { get; set; }
    public OrderEntity Order { get; set; } = null!;
}