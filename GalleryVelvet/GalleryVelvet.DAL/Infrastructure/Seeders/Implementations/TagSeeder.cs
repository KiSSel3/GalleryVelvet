using GalleryVelvet.DAL.Infrastructure.Seeders.Interfaces;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.DAL.Infrastructure.Seeders.Implementations;

public sealed class TagSeeder : ISeeder
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (context.Tags.Any())
        {
            return;
        }

        await context.Tags.AddRangeAsync(
            new TagEntity { Name = "Новая коллекция" },
            new TagEntity { Name = "Бестселлеры" }
        );

        await context.SaveChangesAsync();
    }
}
