using GalleryVelvet.BLL.DTOs.Admin;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Admin;

public sealed class EditProductAdminViewModel
{
    public ProductEntity Product { get; set; } = null!;
    public IEnumerable<CategoryEntity> Categories { get; set; } = [];
    public IEnumerable<TagEntity> Tags { get; set; } = [];
    public IEnumerable<SizeEntity> Sizes { get; set; } = [];
    public UpdateProductDto ProductDto { get; set; } = null!;
}