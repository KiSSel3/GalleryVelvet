using GalleryVelvet.Domain.Common;

namespace GalleryVelvet.Domain.Entities;

public sealed class OrderEntity : BaseEntity
{
    public DateTime OrderDate { get; set; }
    
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    
    public required string Email { get; set; }
    
    public required string PhoneNumber { get; set; }
    
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    
    public Guid OrderStatusId { get; set; }
    public OrderStatusEntity OrderStatus = null!;
    
    public ICollection<OrderItemEntity> OrderItems { get; set; } = [];
}