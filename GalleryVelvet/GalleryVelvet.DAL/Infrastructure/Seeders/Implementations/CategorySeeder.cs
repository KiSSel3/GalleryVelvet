using GalleryVelvet.DAL.Infrastructure.Seeders.Interfaces;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.DAL.Infrastructure.Seeders.Implementations;

public sealed class CategorySeeder : ISeeder
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (context.Categories.Any())
        {
            return;
        }

        await context.Categories.AddRangeAsync(
            new CategoryEntity { Name = "Футболки • Топы" },
            new CategoryEntity { Name = "Худи • Свитшоты" },
            new CategoryEntity { Name = "Свитеры • Водолазки" },
            new CategoryEntity { Name = "Рубашки • Блузки" },
            new CategoryEntity { Name = "Платья • Комбинезоны" },
            new CategoryEntity { Name = "Жакеты" },
            new CategoryEntity { Name = "Брюки • Треники" },
            new CategoryEntity { Name = "Юбки • Шорты" },
            new CategoryEntity { Name = "Бельё • Купальники" }
        );

        await context.SaveChangesAsync();
    }
}
