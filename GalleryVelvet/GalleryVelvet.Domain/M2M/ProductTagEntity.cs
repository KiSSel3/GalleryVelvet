using GalleryVelvet.Domain.Common;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Domain.M2M;

public sealed class ProductTagEntity : BaseEntity
{
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;

    public Guid TagId { get; set; }
    public TagEntity Tag { get; set; } = null!;
}