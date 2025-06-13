using GalleryVelvet.BLL.DTOs.User;
using GalleryVelvet.Domain.Entities;
using GalleryVelvet.Presentation.Models.User;
using Mapster;

namespace GalleryVelvet.Presentation.Mappers;

public static class MappingConfig
{
    public static void ConfigureUserMappings()
    {
        TypeAdapterConfig<OrderEntity, UserOrderDto>
            .NewConfig()
            .Map(dest => dest.OrderStatus, src => src.OrderStatus.Name)
            .Map(dest => dest.TotalAmount, src => src.OrderItems.Sum(oi => oi.Product.DiscountPrice ?? oi.Product.Price * oi.Quantity))
            .Map(dest => dest.ItemsCount, src => src.OrderItems.Sum(oi => oi.Quantity))
            .Map(dest => dest.OrderItems, src => src.OrderItems);
        
        TypeAdapterConfig<OrderItemEntity, UserOrderItemDto>
            .NewConfig()
            .Map(dest => dest.ProductName, src => src.Product.Name)
            .Map(dest => dest.ProductImage, src => src.Product.Images.FirstOrDefault() != null ? src.Product.Images.FirstOrDefault()!.Image : null)
            .Map(dest => dest.ImageFormat, src => src.Product.Images.FirstOrDefault() != null ? src.Product.Images.FirstOrDefault()!.Format : null)
            .Map(dest => dest.SizeLabel, src => src.Size.Label);
        
        TypeAdapterConfig<UserOrderDto, OrderHistoryViewModel>
            .NewConfig();
        
        TypeAdapterConfig<UserOrderItemDto, OrderItemViewModel>
            .NewConfig();
    }
}