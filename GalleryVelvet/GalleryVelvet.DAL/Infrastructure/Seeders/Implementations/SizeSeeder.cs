using GalleryVelvet.DAL.Infrastructure.Seeders.Interfaces;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.DAL.Infrastructure.Seeders.Implementations;

public sealed class SizeSeeder : ISeeder
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (context.Sizes.Any())
        {
            return;
        }

        await context.Sizes.AddRangeAsync(
            new SizeEntity { Label = "XS" },
            new SizeEntity { Label = "S" },
            new SizeEntity { Label = "M" },
            new SizeEntity { Label = "L" },
            new SizeEntity { Label = "XL" }
        );

        await context.SaveChangesAsync();
    }
}
