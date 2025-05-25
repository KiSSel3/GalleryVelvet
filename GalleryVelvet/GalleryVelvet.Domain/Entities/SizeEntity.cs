using GalleryVelvet.Domain.Common;
using GalleryVelvet.Domain.M2M;

namespace GalleryVelvet.Domain.Entities;

public sealed class SizeEntity : BaseEntity
{
    public required string Label { get; set; }
    public ICollection<CartItemEntity> CartItems { get; set; } = [];
    public ICollection<OrderItemEntity> OrderItems { get; set; } = [];
    public ICollection<ProductSizeEntity> ProductSizes { get; set; } = [];
}