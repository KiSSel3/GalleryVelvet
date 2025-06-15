using System.Security.Claims;
using GalleryVelvet.BLL.DTOs.Order;
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
    IUserService userService,
    IOrderService orderService) : Controller
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
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrder(CreateOrderDto model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            var userId = GetCurrentUserId();
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

            return View("GetCartByUserId", viewModel);
        }

        try
        {
            var userId = GetCurrentUserId();
            
            var order = await orderService.CreateOrderFromCartAsync(userId, model, cancellationToken);

            TempData["Success"] = "Заказ успешно создан!";
            TempData["OrderId"] = order.Id.ToString();
            
            return RedirectToAction("OrderSuccess", new { orderId = order.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            
            var userId = GetCurrentUserId();
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
            catch (Exception ex2)
            {
                Console.WriteLine($"Error getting user profile: {ex2.Message}");
            }

            var viewModel = new CartPageViewModel
            {
                CartItems = cartItems,
                UserProfile = userProfile
            };

            return View("GetCartByUserId", viewModel);
        }
    }

    public async Task<IActionResult> OrderSuccess(Guid orderId, CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var order = await orderService.GetOrderByIdAsync(orderId, cancellationToken);

            if (order == null || order.UserId != userId)
            {
                return NotFound("Заказ не найден");
            }

            return View(order);
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
            var userId = GetCurrentUserId();
            
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
            var userId = GetCurrentUserId();
            
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
            var userId = GetCurrentUserId();
            
            await cartItemService.ClearCartAsync(userId, cancellationToken);

            return Redirect("/Cart/GetCartByUserId");
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("Пользователь не авторизован");
        }
        return userId;
    }
}