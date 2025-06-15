using GalleryVelvet.BLL.RequestFeatures;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Admin;

public sealed class UsersAdminViewModel
{
    public PagedList<UserEntity> Users { get; set; } = null!;
    public string? SearchTerm { get; set; }
}