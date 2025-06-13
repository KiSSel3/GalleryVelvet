using GalleryVelvet.BLL.DTOs.User;

namespace GalleryVelvet.BLL.Services.Interfaces;

public interface IUserService
{
    Task<UserProfileDto> GetUserProfileAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileDto updateDto, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserOrderDto>> GetUserOrdersAsync(Guid userId, CancellationToken cancellationToken = default);
}