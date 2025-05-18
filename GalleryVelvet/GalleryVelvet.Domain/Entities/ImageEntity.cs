using GalleryVelvet.Domain.Common;

namespace GalleryVelvet.Domain.Entities;

public sealed class ImageEntity : BaseEntity
{
    public byte[] Image { get; set; } = [];
    
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
}