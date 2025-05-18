using GalleryVelvet.DAL.DI;
using GalleryVelvet.DAL.Infrastructure;
using GalleryVelvet.DAL.Infrastructure.Seeders.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDataAccessLayerServices(builder.Configuration);

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