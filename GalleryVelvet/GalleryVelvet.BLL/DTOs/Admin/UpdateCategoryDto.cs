using System.ComponentModel.DataAnnotations;

namespace GalleryVelvet.BLL.DTOs.Admin;

public sealed class UpdateCategoryDto
{
    [Required(ErrorMessage = "Название категории обязательно")]
    [StringLength(100, ErrorMessage = "Название не может быть длиннее 100 символов")]
    public required string Name { get; set; }
}