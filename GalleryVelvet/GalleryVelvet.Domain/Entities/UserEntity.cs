using GalleryVelvet.Domain.Common;
using GalleryVelvet.Domain.M2M;

namespace GalleryVelvet.Domain.Entities;

public sealed class UserEntity : BaseEntity
{
    public required string Login { get; set; }
    public required string Password { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string? Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public ICollection<RoleEntity> Roles { get; set; } = [];
    public ICollection<OrderEntity> Orders { get; set; } = [];
    public ICollection<CartItemEntity> CartItems { get; set; } = [];
    public ICollection<UserRoleEntity> UserRoles { get; set; } = [];
}