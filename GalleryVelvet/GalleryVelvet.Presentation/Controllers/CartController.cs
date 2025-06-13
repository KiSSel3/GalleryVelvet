using System.Security.Claims;
using GalleryVelvet.BLL.DTOs.User;
using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.Presentation.Models.Cart;
using GalleryVelvet.Presentation.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalleryVelvet.Presentation.Controllers;

[Authorize]
public sealed class CartController(
    ICartItemService cartItemService, 
    IUserService userService) : Controller
{
    public async Task<IActionResult> GetCartByUserId(CancellationToken cancellationToken)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new Exception("Invalid user id");
            }
            
            var cartItems = await cartItemService.GetCartItemsAsync(userId, cancellationToken);
            
            UserProfileDto? userProfile = null;
            try
            {
                var user = await userService.GetUserProfileAsync(userId, cancellationToken);
                if (user is not null)
                {
                    userProfile = new UserProfileDto
                    {
                        Login = user.Login,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user profile: {ex.Message}");
            }

            var viewModel = new CartPageViewModel
            {
                CartItems = cartItems,
                UserProfile = userProfile
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }
    
    public async Task<IActionResult> AddToCart(
        Guid productId,
        Guid sizeId,
        int quantity = 1,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new Exception("Invalid user id");
            }
            
            await cartItemService.AddToCartAsync(
                userId,
                productId,
                sizeId,
                quantity,
                cancellationToken
            );

            return Redirect("/Cart/GetCartByUserId");
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }
    
    public async Task<IActionResult> RemoveItem(Guid cartItemId, CancellationToken cancellationToken)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new Exception("Invalid user id");
            }
            
            await cartItemService.RemoveFromCartAsync(userId, cartItemId, cancellationToken);

            return Redirect("/Cart/GetCartByUserId");
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }
    
    public async Task<IActionResult> ClearCart(CancellationToken cancellationToken)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new Exception("Invalid user id");
            }
            
            await cartItemService.ClearCartAsync(userId, cancellationToken);

            return Redirect("/Cart/GetCartByUserId");
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }
}