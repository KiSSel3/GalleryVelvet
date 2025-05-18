using GalleryVelvet.Domain.Common;

namespace GalleryVelvet.Domain.Entities;

public sealed class SizeEntity : BaseEntity
{
    public required string Label { get; set; }
}