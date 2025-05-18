using GalleryVelvet.Domain.Common;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Domain.M2M;

public sealed class ProductSizeEntity : BaseEntity
{
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;

    public Guid SizeId { get; set; }
    public SizeEntity Size { get; set; } = null!;
}