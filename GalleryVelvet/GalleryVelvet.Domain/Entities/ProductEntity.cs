using GalleryVelvet.Domain.Common;
using GalleryVelvet.Domain.M2M;

namespace GalleryVelvet.Domain.Entities;

public sealed class ProductEntity : BaseEntity
{
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    public string? CompositionAndCare { get; set; }
    
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    
    public Guid CategoryId { get; set; }
    public CategoryEntity Category { get; set; } = null!;

    public ICollection<ImageEntity> Images { get; set; } = [];
    public ICollection<TagEntity> Tags { get; set; } = [];
    public ICollection<SizeEntity> Sizes { get; set; } = [];
    public ICollection<CartItemEntity> CartItems { get; set; } = [];
    public ICollection<OrderItemEntity> OrderItems { get; set; } = [];
    public ICollection<ProductTagEntity> ProductTags { get; set; }
    public ICollection<ProductSizeEntity> ProductSizes { get; set; } = [];
}