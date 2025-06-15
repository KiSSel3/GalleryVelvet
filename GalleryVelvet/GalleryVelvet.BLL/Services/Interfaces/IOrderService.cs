using GalleryVelvet.BLL.DTOs.Order;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.BLL.Services.Interfaces;

public interface IOrderService
{
    Task<OrderEntity> CreateOrderFromCartAsync(
        Guid userId,
        CreateOrderDto createOrderDto,
        CancellationToken cancellationToken = default);
        
    Task<OrderEntity?> GetOrderByIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);
}