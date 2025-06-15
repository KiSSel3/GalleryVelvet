using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Admin;

public sealed class OrderDetailsAdminViewModel
{
    public OrderEntity Order { get; set; } = null!;
    public IEnumerable<OrderStatusEntity> OrderStatuses { get; set; } = [];
}