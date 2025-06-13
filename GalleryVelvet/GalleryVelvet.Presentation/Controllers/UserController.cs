using System.Security.Claims;
using GalleryVelvet.BLL.DTOs.User;
using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.Presentation.Models.User;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalleryVelvet.Presentation.Controllers;

[Authorize]
public sealed class UserController(IUserService userService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string tab = "profile", CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            
            var profile = await userService.GetUserProfileAsync(userId, cancellationToken);
            var orders = await userService.GetUserOrdersAsync(userId, cancellationToken);

            var viewModel = new UserProfilePageViewModel
            {
                Profile = profile.Adapt<ProfileViewModel>(),
                Orders = orders.Adapt<IEnumerable<OrderHistoryViewModel>>(),
                ActiveTab = tab
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(ProfileViewModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            var userId = GetCurrentUserId();
            var orders = await userService.GetUserOrdersAsync(userId, cancellationToken);
            
            var viewModel = new UserProfilePageViewModel
            {
                Profile = model,
                Orders = orders.Adapt<IEnumerable<OrderHistoryViewModel>>(),
                ActiveTab = "profile"
            };

            return View("Index", viewModel);
        }

        try
        {
            var userId = GetCurrentUserId();
            var updateDto = model.Adapt<UpdateUserProfileDto>();
            
            await userService.UpdateUserProfileAsync(userId, updateDto, cancellationToken);
            
            TempData["Success"] = "Профиль успешно обновлен";
            return RedirectToAction("Index", new { tab = "profile" });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            var userId = GetCurrentUserId();
            var orders = await userService.GetUserOrdersAsync(userId, cancellationToken);
            
            var viewModel = new UserProfilePageViewModel
            {
                Profile = model,
                Orders = orders.Adapt<IEnumerable<OrderHistoryViewModel>>(),
                ActiveTab = "profile"
            };

            return View("Index", viewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Orders(CancellationToken cancellationToken = default)
    {
        return await Index("orders", cancellationToken);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Пользователь не авторизован");
        }
        
        return userId;
    }
}