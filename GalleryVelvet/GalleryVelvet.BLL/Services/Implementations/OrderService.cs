using GalleryVelvet.BLL.DTOs.Order;
using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.DAL.Repositories.Interfaces;
using GalleryVelvet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalleryVelvet.BLL.Services.Implementations;

public sealed class OrderService(
    IOrderRepository orderRepository,
    ICartItemRepository cartItemRepository,
    IOrderStatusRepository orderStatusRepository) : IOrderService
{
    private const string DefaultOrderStatusName = "В обработке";

    public async Task<OrderEntity> CreateOrderFromCartAsync(
        Guid userId,
        CreateOrderDto createOrderDto,
        CancellationToken cancellationToken = default)
    {
        var cartItems = await cartItemRepository
            .GetQueryable(
                filter: ci => ci.UserId == userId,
                includeBuilder: q => q
                    .Include(ci => ci.Product)
                    .Include(ci => ci.Size))
            .ToListAsync(cancellationToken);

        if (!cartItems.Any())
        {
            throw new InvalidOperationException("Корзина пуста. Нельзя создать заказ.");
        }
        
        var defaultStatus = await orderStatusRepository.GetFirstOrDefaultAsync(
            filter: status => status.Name == DefaultOrderStatusName,
            cancellationToken: cancellationToken);

        if (defaultStatus is null)
        {
            throw new InvalidOperationException($"Статус заказа '{DefaultOrderStatusName}' не найден в системе.");
        }
        
        var order = new OrderEntity
        {
            OrderDate = DateTime.UtcNow,
            FirstName = createOrderDto.FirstName.Trim(),
            LastName = createOrderDto.LastName.Trim(),
            Email = createOrderDto.Email.Trim().ToLowerInvariant(),
            PhoneNumber = createOrderDto.PhoneNumber.Trim(),
            UserId = userId,
            OrderStatusId = defaultStatus.Id,
            OrderItems = cartItems.Select(ci => new OrderItemEntity
            {
                ProductId = ci.ProductId,
                SizeId = ci.SizeId,
                Quantity = ci.Quantity
            }).ToList()
        };
        
        await orderRepository.CreateAsync(order, cancellationToken);
        
        await cartItemRepository.DeleteByUserIdAsync(userId, cancellationToken);

        return order;
    }

    public Task<OrderEntity?> GetOrderByIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return orderRepository.GetByIdAsync(
            orderId,
            includeBuilder: q => q
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Images)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Size),
            cancellationToken);
    }
}