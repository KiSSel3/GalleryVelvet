namespace GalleryVelvet.BLL.DTOs.Cart;

public sealed class AddCartItemDto
{
    public Guid ProductId { get; set; }
    public Guid SizeId { get; set; }
    public int Quantity { get; set; }
}