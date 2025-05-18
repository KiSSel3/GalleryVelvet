using GalleryVelvet.Domain.Common;

namespace GalleryVelvet.Domain.Entities;

public sealed class CategoryEntity : BaseEntity
{
    public required string Name { get; set; }

    public ICollection<ProductEntity> Products { get; set; } = [];
}