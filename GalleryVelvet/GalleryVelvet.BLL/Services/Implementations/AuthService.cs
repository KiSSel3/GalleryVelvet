using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using GalleryVelvet.BLL.DTOs.Authorization;
using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.DAL.Repositories.Interfaces;
using GalleryVelvet.Domain.Entities;
using GalleryVelvet.Domain.M2M;
using Mapster;

namespace GalleryVelvet.BLL.Services.Implementations;

public sealed class AuthService(
    IUserRepository userRepository,
    IRoleRepository roleRepository) : IAuthService
{
    private const string ClaimsIdentityName = "Authentication";
    private const string DefaultRoleName = "User";
    
    public async Task<ClaimsIdentity> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var targetUser = await userRepository.GetFirstOrDefaultAsync(
            u => u.Login == loginDto.Login && u.Password == loginDto.Password,
            q => q
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role),
            cancellationToken);

        if (targetUser is null)
        {
            throw new Exception("Неверный логин или пароль.");
        }

        var claims = BuildClaims(targetUser);
        
        return new ClaimsIdentity(claims, ClaimsIdentityName);
    }

    public async Task<ClaimsIdentity> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetFirstOrDefaultAsync(
            u => u.Login == registerDto.Login && u.Password == registerDto.Password,
            cancellationToken: cancellationToken);
        
        if (user is not null)
        {
            throw new Exception("Пользователь с таким логином уже существует.");
        }
        
        user = registerDto.Adapt<UserEntity>();

        await userRepository.CreateAsync(user, cancellationToken);

        await AssignRoleToUser(user, DefaultRoleName, cancellationToken);
        
        var claims = BuildClaims(user);
        
        return new ClaimsIdentity(claims, ClaimsIdentityName);
    }

    private async Task AssignRoleToUser(UserEntity user, string roleName, CancellationToken cancellationToken)
    {
        var defaultRole = await roleRepository.GetFirstOrDefaultAsync(
            r => r.Name == roleName,
            cancellationToken: cancellationToken);

        if (defaultRole is null)
        {
            throw new Exception($"Роль с именем '{roleName}' не найдена в системе.");
        }
        
        user.UserRoles =
        [
            new UserRoleEntity
            {
                UserId = user.Id,
                RoleId = defaultRole.Id
            }
        ];
        
        await userRepository.UpdateAsync(user, cancellationToken);
    }

    private IEnumerable<Claim> BuildClaims(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Login),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        claims.AddRange(user.UserRoles.Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name)));

        return claims;
    }
}