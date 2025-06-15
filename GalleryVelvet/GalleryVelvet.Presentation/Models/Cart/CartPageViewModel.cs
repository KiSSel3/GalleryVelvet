using GalleryVelvet.BLL.DTOs.Order;
using GalleryVelvet.BLL.DTOs.User;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Cart;

public sealed class CartPageViewModel
{
    public IEnumerable<CartItemEntity> CartItems { get; set; } = [];
    public UserProfileDto? UserProfile { get; set; }
    public CreateOrderDto OrderData { get; set; } = new CreateOrderDto
    {
        FirstName = "",
        LastName = "",
        Email = "",
        PhoneNumber = "",
        DeliveryType = "delivery"
    };
}