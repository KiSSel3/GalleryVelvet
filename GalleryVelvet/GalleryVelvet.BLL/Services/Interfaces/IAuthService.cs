using System.Security.Claims;
using GalleryVelvet.BLL.DTOs.Authorization;

namespace GalleryVelvet.BLL.Services.Interfaces;

public interface IAuthService
{
    Task<ClaimsIdentity> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
    
    Task<ClaimsIdentity> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);
}