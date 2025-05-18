using GalleryVelvet.DAL.Infrastructure.Seeders.Interfaces;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.DAL.Infrastructure.Seeders.Implementations;

public sealed class OrderStatusSeeder : ISeeder
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (context.OrderStatuses.Any())
        {
            return;
        }

        await context.OrderStatuses.AddRangeAsync(
            new OrderStatusEntity { Name = "Новый" },
            new OrderStatusEntity { Name = "В обработке" },
            new OrderStatusEntity { Name = "Отменён" },
            new OrderStatusEntity { Name = "Выполнен" }
        );

        await context.SaveChangesAsync();
    }
}
