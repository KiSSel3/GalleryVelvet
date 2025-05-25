using GalleryVelvet.BLL.Enums;
using GalleryVelvet.BLL.RequestFeatures;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Product;

public class ProductCatalogViewModel
{
    public PagedList<ProductEntity> Products { get; set; } = null!;
    
    public Guid? SelectedCategoryId { get; set; }
    
    public string? SearchQuery { get; set; }

    public ProductSortOrder SortOrder { get; set; } = ProductSortOrder.None;
    public bool OnlyDiscounted { get; set; }
    public Guid? SelectedTagId { get; set; }
}