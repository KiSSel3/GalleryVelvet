namespace GalleryVelvet.BLL.DTOs.User;

public sealed class UserOrderItemDto
{
    public string ProductName { get; set; } = string.Empty;
    public string? ProductImage { get; set; }
    public string? ImageFormat { get; set; }
    public string SizeLabel { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}