using GalleryVelvet.Domain.Common;
using GalleryVelvet.Domain.M2M;

namespace GalleryVelvet.Domain.Entities;

public sealed class TagEntity : BaseEntity
{
    public required string Name { get; set; }
    
    public ICollection<ProductTagEntity> ProductTags { get; set; }
}