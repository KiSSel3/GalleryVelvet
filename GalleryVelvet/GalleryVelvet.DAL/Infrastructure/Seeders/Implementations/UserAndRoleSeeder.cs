using GalleryVelvet.DAL.Infrastructure.Seeders.Interfaces;
using GalleryVelvet.Domain.Entities;
using GalleryVelvet.Domain.M2M;

namespace GalleryVelvet.DAL.Infrastructure.Seeders.Implementations;

public sealed class UserAndRoleSeeder : ISeeder
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new RoleEntity { Name = "Admin" },
                new RoleEntity { Name = "User" }
            );

            await context.SaveChangesAsync();
        }

        if (!context.Users.Any())
        {
            var admin = new UserEntity
            {
                Login = "admin",
                Password = "admin",
            };

            var user = new UserEntity
            {
                Login = "user",
                Password = "user",
            };

            context.Users.AddRange(admin, user);
            
            await context.SaveChangesAsync();

            var adminRole = context.Roles.First(r => r.Name == "Admin");
            var userRole = context.Roles.First(r => r.Name == "User");

            context.UserRoles.AddRange(
                new UserRoleEntity { UserId = admin.Id, RoleId = adminRole.Id },
                new UserRoleEntity { UserId = user.Id, RoleId = userRole.Id }
            );

            await context.SaveChangesAsync();
        }
    }
}
