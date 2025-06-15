using GalleryVelvet.BLL.RequestFeatures;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Admin;

public sealed class TagsAdminViewModel
{
    public PagedList<TagEntity> Tags { get; set; } = null!;
    public string? SearchTerm { get; set; }
}