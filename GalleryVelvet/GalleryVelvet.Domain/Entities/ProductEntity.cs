using GalleryVelvet.Domain.Common;

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
}