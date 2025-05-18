using GalleryVelvet.Domain.Common;

namespace GalleryVelvet.Domain.Entities;

public sealed class RoleEntity : BaseEntity
{
    public required string Name { get; set; }
}