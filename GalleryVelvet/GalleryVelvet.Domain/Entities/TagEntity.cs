using GalleryVelvet.Domain.Common;

namespace GalleryVelvet.Domain.Entities;

public sealed class TagEntity : BaseEntity
{
    public required string Name { get; set; }
}