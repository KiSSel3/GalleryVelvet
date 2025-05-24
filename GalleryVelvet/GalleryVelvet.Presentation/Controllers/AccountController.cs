using System.Security.Claims;
using GalleryVelvet.BLL.DTOs.Authorization;
using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.Presentation.Models.Account;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalleryVelvet.Presentation.Controllers;

public sealed class AccountController(IAuthService authService) : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var loginDto = model.Adapt<LoginDto>();
            
            var response = await authService.LoginAsync(loginDto, cancellationToken);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(response));

            return Redirect("/");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("AuthorizationError", ex.Message);
            
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var registerDto = model.Adapt<RegisterDto>();
            
            var response = await authService.RegisterAsync(registerDto, cancellationToken);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(response));

            return Redirect("/");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("AuthorizationError", ex.Message);
            
            return View(model);
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        return Redirect("/");
    }
}