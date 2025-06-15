using GalleryVelvet.BLL.RequestFeatures;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Admin;

public sealed class ProductsAdminViewModel
{
    public PagedList<ProductEntity> Products { get; set; } = null!;
    public IEnumerable<CategoryEntity> Categories { get; set; } = [];
    public string? SearchTerm { get; set; }
    public Guid? SelectedCategoryId { get; set; }
    public bool IncludeDeleted { get; set; }
}