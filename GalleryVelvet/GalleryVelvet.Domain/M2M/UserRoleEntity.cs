using GalleryVelvet.Domain.Common;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Domain.M2M;

public sealed class UserRoleEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    
    public Guid RoleId { get; set; }
    public RoleEntity Role { get; set; } = null!;
}