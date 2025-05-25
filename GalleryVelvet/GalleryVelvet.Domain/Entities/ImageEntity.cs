using GalleryVelvet.Domain.Common;

namespace GalleryVelvet.Domain.Entities;

public sealed class ImageEntity : BaseEntity
{
    public required string Image { get; set; }
    public required string Format { get; set; }
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
}