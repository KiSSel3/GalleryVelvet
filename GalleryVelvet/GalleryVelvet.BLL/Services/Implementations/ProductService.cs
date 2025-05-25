using GalleryVelvet.BLL.Enums;
using GalleryVelvet.BLL.RequestFeatures;
using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.DAL.Repositories.Interfaces;
using GalleryVelvet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalleryVelvet.BLL.Services.Implementations;

public sealed class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<PagedList<ProductEntity>> GetPagedProductsAsync(
        int pageNumber,
        int pageSize,
        Guid? categoryId,
        string? search,
        ProductSortOrder sortOrder,
        bool onlyDiscounted = false,
        Guid? tagId = null,
        CancellationToken cancellationToken = default)
    {
        var query = productRepository.GetQueryable(
            filter: p =>
                (string.IsNullOrWhiteSpace(search) || p.Name.ToLower().Contains(search.ToLower())) &&
                (!categoryId.HasValue || p.CategoryId == categoryId),
            includeBuilder: q => q.Include(p => p.Images));

        if (onlyDiscounted)
        {
            query = query.Where(p => p.DiscountPrice != null && p.DiscountPrice < p.Price);
        }
        
        if (tagId.HasValue)
        {
            query = query.Where(p => p.ProductTags.Any(pt => pt.TagId == tagId.Value));
        }
        
        query = sortOrder switch
        {
            ProductSortOrder.PriceLowToHigh => query.OrderBy(p => p.DiscountPrice ?? p.Price),
            ProductSortOrder.PriceHighToLow => query.OrderByDescending(p => p.DiscountPrice ?? p.Price),
            _ => query
        };

        return await PagedList<ProductEntity>
            .ToPagedListAsync(
                query,
                pageNumber,
                pageSize,
                cancellationToken);
    }

    public Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return productRepository.GetByIdAsync(
            id,
            includeBuilder: query => query
                .Include(p => p.Images)
                .Include(p => p.ProductSizes)
                    .ThenInclude(ps => ps.Size),
            cancellationToken);
    }
}