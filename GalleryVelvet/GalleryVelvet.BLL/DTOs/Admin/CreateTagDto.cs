using System.ComponentModel.DataAnnotations;

namespace GalleryVelvet.BLL.DTOs.Admin;

public sealed class CreateTagDto
{
    [Required(ErrorMessage = "Название тега обязательно")]
    [StringLength(50, ErrorMessage = "Название не может быть длиннее 50 символов")]
    public required string Name { get; set; }
}