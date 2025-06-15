using System.ComponentModel.DataAnnotations;

namespace GalleryVelvet.BLL.DTOs.Admin;

public sealed class UpdateUserRolesDto
{
    [Required(ErrorMessage = "Список ролей обязателен")]
    public required List<Guid> RoleIds { get; set; }
}