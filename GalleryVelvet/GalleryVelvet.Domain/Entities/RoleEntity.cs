using GalleryVelvet.Domain.Common;
using GalleryVelvet.Domain.M2M;

namespace GalleryVelvet.Domain.Entities;

public sealed class RoleEntity : BaseEntity
{
    public required string Name { get; set; }

    public ICollection<UserEntity> Users { get; set; } = [];
    public ICollection<UserRoleEntity> UserRoles { get; set; } = [];
}