using GalleryVelvet.BLL.DTOs.Category;

namespace GalleryVelvet.BLL.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryToSidebarDto>> GetAllCategoriesToSidebarAsync(CancellationToken cancellationToken = default);
}