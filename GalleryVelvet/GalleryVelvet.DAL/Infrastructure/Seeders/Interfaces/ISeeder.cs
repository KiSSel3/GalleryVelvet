namespace GalleryVelvet.DAL.Infrastructure.Seeders.Interfaces;

public interface ISeeder
{
    Task SeedAsync(AppDbContext context);
}
