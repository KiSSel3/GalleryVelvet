using GalleryVelvet.BLL.DI;
using GalleryVelvet.DAL.DI;
using GalleryVelvet.DAL.Infrastructure;
using GalleryVelvet.DAL.Infrastructure.Seeders.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Authentication";
        options.LoginPath = "/Account/Authorization";
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = true;
    });

builder.Services
    .AddDataAccessLayerServices(builder.Configuration)
    .AddBusinessLogicLayerServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var seeders = scope.ServiceProvider.GetServices<ISeeder>();

    foreach (var seeder in seeders)
    {
        await seeder.SeedAsync(context);
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();