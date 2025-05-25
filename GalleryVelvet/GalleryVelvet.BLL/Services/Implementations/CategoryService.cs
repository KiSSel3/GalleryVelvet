using GalleryVelvet.BLL.DTOs.Category;
using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GalleryVelvet.BLL.Services.Implementations;

public sealed class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<IEnumerable<CategoryToSidebarDto>> GetAllCategoriesToSidebarAsync(CancellationToken cancellationToken = default)
    {
        var categoriesToSidebar = await categoryRepository.GetQueryable()
            .Select(c => new CategoryToSidebarDto
            {
                Id = c.Id,
                Name = c.Name,
                ProductCount = c.Products.Count(p => !p.IsDeleted)
            })
            .ToListAsync(cancellationToken);

        return categoriesToSidebar;
    }
}