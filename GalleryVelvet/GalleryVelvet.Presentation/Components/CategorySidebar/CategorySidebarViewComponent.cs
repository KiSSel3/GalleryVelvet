using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.Presentation.Models.Category;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace GalleryVelvet.Presentation.Components.CategorySidebar;

public class CategorySidebarViewComponent(ICategoryService categoryService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await categoryService.GetAllCategoriesToSidebarAsync();

        var viewModels = categories.Adapt<IEnumerable<CategoryToSidebarViewModel>>();
        
        return View(viewModels);
    }
}
