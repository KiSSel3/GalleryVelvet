using GalleryVelvet.Domain.Interfaces;

namespace GalleryVelvet.Domain.Common;

public abstract class BaseEntity : IHasId, ISoftDeletable
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
}