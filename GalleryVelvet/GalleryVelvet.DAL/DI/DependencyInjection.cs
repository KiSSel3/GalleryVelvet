using GalleryVelvet.DAL.Infrastructure;
using GalleryVelvet.DAL.Infrastructure.Seeders.Implementations;
using GalleryVelvet.DAL.Infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GalleryVelvet.DAL.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigurePostgreSql(configuration)
            .AddSeeders()
            .AddRepositories();
        
        return services;
    }
    
    private static IServiceCollection ConfigurePostgreSql(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        string? dataBaseConnection = configuration.GetConnectionString("PostrgeSql");
        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(dataBaseConnection));

        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Примеры регистрации репозиториев:
        // services.AddScoped<IProductRepository, ProductRepository>();
        // services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    private static IServiceCollection AddSeeders(this IServiceCollection services)
    {
        services
            .AddScoped<ISeeder, CategorySeeder>()
            .AddScoped<ISeeder, TagSeeder>()
            .AddScoped<ISeeder, SizeSeeder>()
            .AddScoped<ISeeder, UserAndRoleSeeder>()
            .AddScoped<ISeeder, OrderStatusSeeder>();

        return services;
    }
}