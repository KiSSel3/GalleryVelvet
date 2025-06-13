namespace GalleryVelvet.Presentation.Models.User;

public sealed class OrderHistoryViewModel
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int ItemsCount { get; set; }
    public IEnumerable<OrderItemViewModel> OrderItems { get; set; } = [];
}