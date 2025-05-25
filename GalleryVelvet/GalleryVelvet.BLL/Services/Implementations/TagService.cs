using GalleryVelvet.BLL.DTOs.Tag;
using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GalleryVelvet.BLL.Services.Implementations;

public sealed class TagService(ITagRepository tagRepository) : ITagService
{
    public async Task<IEnumerable<TagToSidebarDto>> GetTagsToSidebarAsync(CancellationToken cancellationToken = default)
    {
        var tags = await tagRepository
            .GetQueryable()
            .Select(t => new TagToSidebarDto
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToListAsync(cancellationToken);

        return tags;
    }
}