using GalleryVelvet.BLL.DTOs.Admin;
using GalleryVelvet.BLL.RequestFeatures;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.BLL.Services.Interfaces;

public interface IAdminService
{
    Task<PagedList<OrderEntity>> GetPagedOrdersAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        Guid? statusId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);
    
    Task<OrderEntity?> GetOrderDetailsAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<bool> UpdateOrderStatusAsync(Guid orderId, Guid statusId, CancellationToken cancellationToken = default);
    
    Task<PagedList<UserEntity>> GetPagedUsersAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    
    Task<UserEntity?> GetUserDetailsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> UpdateUserRolesAsync(Guid userId, List<Guid> roleIds, CancellationToken cancellationToken = default);
    
    Task<PagedList<CategoryEntity>> GetPagedCategoriesAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    
    Task<CategoryEntity> CreateCategoryAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<CategoryEntity?> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<PagedList<TagEntity>> GetPagedTagsAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
    
    Task<TagEntity> CreateTagAsync(CreateTagDto dto, CancellationToken cancellationToken = default);
    Task<TagEntity?> UpdateTagAsync(Guid id, UpdateTagDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteTagAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<PagedList<ProductEntity>> GetPagedProductsForAdminAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        Guid? categoryId = null,
        bool? includeDeleted = false,
        CancellationToken cancellationToken = default);
    
    Task<ProductEntity> CreateProductAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
    Task<ProductEntity?> UpdateProductAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(Guid id, bool hardDelete = false, CancellationToken cancellationToken = default);
    Task<bool> RestoreProductAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<OrderStatusEntity>> GetAllOrderStatusesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<RoleEntity>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryEntity>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TagEntity>> GetAllTagsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SizeEntity>> GetAllSizesAsync(CancellationToken cancellationToken = default);
}