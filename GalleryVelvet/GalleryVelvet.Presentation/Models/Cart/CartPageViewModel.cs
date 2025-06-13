using GalleryVelvet.BLL.DTOs.User;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Cart;

public sealed class CartPageViewModel
{
    public IEnumerable<CartItemEntity> CartItems { get; set; } = [];
    public UserProfileDto? UserProfile { get; set; }
}