using GalleryVelvet.BLL.Enums;
using GalleryVelvet.BLL.RequestFeatures;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.BLL.Services.Interfaces;

public interface IProductService
{
    Task<PagedList<ProductEntity>> GetPagedProductsAsync(
        int pageNumber,
        int pageSize,
        Guid? categoryId,
        string? search,
        ProductSortOrder sortOrder,
        bool onlyDiscounted = false,
        Guid? tagId = null,
        CancellationToken cancellationToken = default);

    Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}