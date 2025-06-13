namespace GalleryVelvet.BLL.DTOs.User;

public sealed class UserOrderDto
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int ItemsCount { get; set; }
    public IEnumerable<UserOrderItemDto> OrderItems { get; set; } = [];
}
