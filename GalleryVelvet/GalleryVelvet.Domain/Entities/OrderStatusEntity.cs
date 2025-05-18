using GalleryVelvet.Domain.Common;

namespace GalleryVelvet.Domain.Entities;

public sealed class OrderStatusEntity : BaseEntity
{
    public required string Name { get; set; }
}