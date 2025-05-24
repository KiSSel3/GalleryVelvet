using GalleryVelvet.BLL.Services.Implementations;
using GalleryVelvet.BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GalleryVelvet.BLL.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();

        return services;
    }
    
    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        
        return services;
    }
}