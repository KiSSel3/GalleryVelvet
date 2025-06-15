using GalleryVelvet.BLL.DTOs.Admin;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Admin;

public sealed class CreateProductAdminViewModel
{
    public IEnumerable<CategoryEntity> Categories { get; set; } = [];
    public IEnumerable<TagEntity> Tags { get; set; } = [];
    public IEnumerable<SizeEntity> Sizes { get; set; } = [];
    public CreateProductDto? Product { get; set; }
}