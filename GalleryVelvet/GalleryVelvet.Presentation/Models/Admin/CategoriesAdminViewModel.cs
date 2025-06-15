using GalleryVelvet.BLL.RequestFeatures;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Admin;

public sealed class CategoriesAdminViewModel
{
    public PagedList<CategoryEntity> Categories { get; set; } = null!;
    public string? SearchTerm { get; set; }
}