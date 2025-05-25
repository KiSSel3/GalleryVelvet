using GalleryVelvet.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GalleryVelvet.Presentation.Components.TagSidebar;

public sealed class TagSidebarViewComponent(ITagService tagService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var tags = await tagService.GetTagsToSidebarAsync();
        
        return View(tags);
    }
}