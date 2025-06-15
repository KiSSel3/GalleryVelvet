using GalleryVelvet.BLL.DTOs.Admin;
using GalleryVelvet.BLL.RequestFeatures;
using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.DAL.Repositories.Interfaces;
using GalleryVelvet.Domain.Entities;
using GalleryVelvet.Domain.M2M;
using Microsoft.EntityFrameworkCore;

namespace GalleryVelvet.BLL.Services.Implementations;

public sealed class AdminService(
    IOrderRepository orderRepository,
    IUserRepository userRepository,
    ICategoryRepository categoryRepository,
    ITagRepository tagRepository,
    IProductRepository productRepository,
    IOrderStatusRepository orderStatusRepository,
    IRoleRepository roleRepository,
    ISizeRepository sizeRepository) : IAdminService
{
    public async Task<PagedList<OrderEntity>> GetPagedOrdersAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        Guid? statusId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = orderRepository.GetQueryable(
            filter: o =>
                (string.IsNullOrWhiteSpace(searchTerm) ||
                 o.FirstName.Contains(searchTerm) ||
                 o.LastName.Contains(searchTerm) ||
                 o.Email.Contains(searchTerm) ||
                 o.PhoneNumber.Contains(searchTerm)) &&
                (!statusId.HasValue || o.OrderStatusId == statusId) &&
                (!fromDate.HasValue || o.OrderDate >= fromDate) &&
                (!toDate.HasValue || o.OrderDate <= toDate),
            orderBy: q => q.OrderByDescending(o => o.OrderDate),
            includeBuilder: q => q
                .Include(o => o.OrderStatus)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product));

        return await PagedList<OrderEntity>.ToPagedListAsync(query, pageNumber, pageSize, cancellationToken);
    }

    public Task<OrderEntity?> GetOrderDetailsAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return orderRepository.GetByIdAsync(
            orderId,
            includeBuilder: q => q
                .Include(o => o.OrderStatus)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Images)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Size),
            cancellationToken);
    }

    public async Task<bool> UpdateOrderStatusAsync(Guid orderId, Guid statusId, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken: cancellationToken);
        if (order == null) return false;

        var status = await orderStatusRepository.GetByIdAsync(statusId, cancellationToken: cancellationToken);
        if (status == null) return false;

        order.OrderStatusId = statusId;
        await orderRepository.UpdateAsync(order, cancellationToken);
        return true;
    }
    
    public async Task<PagedList<UserEntity>> GetPagedUsersAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = userRepository.GetQueryable(
            filter: u =>
                string.IsNullOrWhiteSpace(searchTerm) ||
                u.Login.Contains(searchTerm) ||
                u.FirstName.Contains(searchTerm) ||
                u.LastName.Contains(searchTerm) ||
                u.Email.Contains(searchTerm),
            orderBy: q => q.OrderBy(u => u.Login),
            includeBuilder: q => q
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role));

        return await PagedList<UserEntity>.ToPagedListAsync(query, pageNumber, pageSize, cancellationToken);
    }

    public Task<UserEntity?> GetUserDetailsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return userRepository.GetByIdAsync(
            userId,
            includeBuilder: q => q
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role),
            cancellationToken);
    }

    public async Task<bool> UpdateUserRolesAsync(Guid userId, List<Guid> roleIds, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(
            userId,
            includeBuilder: q => q.Include(u => u.UserRoles),
            cancellationToken);

        if (user == null) return false;
        
        var roles = await roleRepository.GetQueryable(filter: r => roleIds.Contains(r.Id))
            .ToListAsync(cancellationToken);

        if (roles.Count != roleIds.Count) return false;
        
        user.UserRoles = roleIds.Select(roleId => new UserRoleEntity
        {
            UserId = userId,
            RoleId = roleId
        }).ToList();

        await userRepository.UpdateAsync(user, cancellationToken);
        return true;
    }
    
    public async Task<PagedList<CategoryEntity>> GetPagedCategoriesAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = categoryRepository.GetQueryable(
            filter: c =>
                string.IsNullOrWhiteSpace(searchTerm) ||
                c.Name.Contains(searchTerm),
            orderBy: q => q.OrderBy(c => c.Name),
            includeBuilder: c => c.Include(p => p.Products));

        return await PagedList<CategoryEntity>.ToPagedListAsync(query, pageNumber, pageSize, cancellationToken);
    }

    public async Task<CategoryEntity> CreateCategoryAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = new CategoryEntity
        {
            Name = dto.Name.Trim(),
        };

        await categoryRepository.CreateAsync(category, cancellationToken);
        return category;
    }

    public async Task<CategoryEntity?> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
        if (category == null) return null;

        category.Name = dto.Name.Trim();

        await categoryRepository.UpdateAsync(category, cancellationToken);
        return category;
    }

    public async Task<bool> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
        if (category == null) return false;
        
        var hasProducts = await productRepository.GetQueryable(filter: p => p.CategoryId == id)
            .AnyAsync(cancellationToken);

        if (hasProducts)
        {
            throw new InvalidOperationException("Нельзя удалить категорию, в которой есть продукты");
        }

        await categoryRepository.HardDeleteAsync(category, cancellationToken);
        return true;
    }
    
    public async Task<PagedList<TagEntity>> GetPagedTagsAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = tagRepository.GetQueryable(
            filter: t =>
                string.IsNullOrWhiteSpace(searchTerm) ||
                t.Name.Contains(searchTerm),
            orderBy: q => q.OrderBy(t => t.Name));

        return await PagedList<TagEntity>.ToPagedListAsync(query, pageNumber, pageSize, cancellationToken);
    }

    public async Task<TagEntity> CreateTagAsync(CreateTagDto dto, CancellationToken cancellationToken = default)
    {
        var tag = new TagEntity
        {
            Name = dto.Name.Trim(),
        };

        await tagRepository.CreateAsync(tag, cancellationToken);
        return tag;
    }

    public async Task<TagEntity?> UpdateTagAsync(Guid id, UpdateTagDto dto, CancellationToken cancellationToken = default)
    {
        var tag = await tagRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
        if (tag == null) return null;

        tag.Name = dto.Name.Trim();

        await tagRepository.UpdateAsync(tag, cancellationToken);
        return tag;
    }

    public async Task<bool> DeleteTagAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tag = await tagRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
        if (tag == null) return false;

        await tagRepository.HardDeleteAsync(tag, cancellationToken);
        return true;
    }
    
    public async Task<PagedList<ProductEntity>> GetPagedProductsForAdminAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        Guid? categoryId = null,
        bool? includeDeleted = false,
        CancellationToken cancellationToken = default)
    {
        var query = productRepository.GetQueryable(
            filter: p =>
                (string.IsNullOrWhiteSpace(searchTerm) || p.Name.Contains(searchTerm)) &&
                (!categoryId.HasValue || p.CategoryId == categoryId) &&
                (includeDeleted == true || !p.IsDeleted),
            orderBy: q => q.OrderBy(p => p.Name),
            includeBuilder: q => q
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductSizes)
                .ThenInclude(ps => ps.Size));

        return await PagedList<ProductEntity>.ToPagedListAsync(query, pageNumber, pageSize, cancellationToken);
    }

    public async Task<ProductEntity> CreateProductAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        var product = new ProductEntity
        {
            Name = dto.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
            Price = dto.Price,
            DiscountPrice = dto.DiscountPrice,
            CategoryId = dto.CategoryId,
            Images = dto.Images.Select(img => new ImageEntity
            {
                Image = img.Image,
                Format = img.Format
            }).ToList(),
            ProductTags = dto.TagIds.Select(tagId => new ProductTagEntity
            {
                TagId = tagId
            }).ToList(),
            ProductSizes = dto.SizeIds.Select(sizeId => new ProductSizeEntity
            {
                SizeId = sizeId
            }).ToList()
        };

        await productRepository.CreateAsync(product, cancellationToken);
        return product;
    }

    public async Task<ProductEntity?> UpdateProductAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(
            id,
            includeBuilder: q => q
                .Include(p => p.Images)
                .Include(p => p.ProductTags)
                .Include(p => p.ProductSizes),
            cancellationToken);

        if (product == null) return null;

        product.Name = dto.Name.Trim();
        product.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        product.Price = dto.Price;
        product.DiscountPrice = dto.DiscountPrice;
        product.CategoryId = dto.CategoryId;

        product.Images = dto.Images.Select(img => new ImageEntity
        {
            ProductId = id,
            Image = img.Image,
            Format = img.Format
        }).ToList();

        product.ProductTags = dto.TagIds.Select(tagId => new ProductTagEntity
        {
            ProductId = id,
            TagId = tagId
        }).ToList();

        product.ProductSizes = dto.SizeIds.Select(sizeId => new ProductSizeEntity
        {
            ProductId = id,
            SizeId = sizeId
        }).ToList();

        await productRepository.UpdateAsync(product, cancellationToken);
        return product;
    }

    public async Task<bool> DeleteProductAsync(Guid id, bool hardDelete = false, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
        if (product == null) return false;

        if (hardDelete)
        {
            await productRepository.HardDeleteAsync(product, cancellationToken);
        }
        else
        {
            await productRepository.SoftDeleteAsync(product, cancellationToken);
        }

        return true;
    }

    public async Task<bool> RestoreProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
        if (product == null) return false;

        product.IsDeleted = false;
        await productRepository.UpdateAsync(product, cancellationToken);
        return true;
    }
    
    public async Task<IEnumerable<OrderStatusEntity>> GetAllOrderStatusesAsync(CancellationToken cancellationToken = default)
    {
        return await orderStatusRepository.GetQueryable().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<RoleEntity>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        return await roleRepository.GetQueryable().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CategoryEntity>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await categoryRepository.GetQueryable().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TagEntity>> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
        return await tagRepository.GetQueryable().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SizeEntity>> GetAllSizesAsync(CancellationToken cancellationToken = default)
    {
        return await sizeRepository.GetQueryable().ToListAsync(cancellationToken);
    }
}