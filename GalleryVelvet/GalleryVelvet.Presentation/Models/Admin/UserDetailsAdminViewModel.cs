using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Admin;

public sealed class UserDetailsAdminViewModel
{
    public UserEntity User { get; set; } = null!;
    public IEnumerable<RoleEntity> AllRoles { get; set; } = [];
    public List<Guid> SelectedRoleIds { get; set; } = [];
}