using GalleryVelvet.BLL.DTOs.Tag;

namespace GalleryVelvet.BLL.Services.Interfaces;

public interface ITagService
{
    Task<IEnumerable<TagToSidebarDto>> GetTagsToSidebarAsync(CancellationToken cancellationToken = default);
}