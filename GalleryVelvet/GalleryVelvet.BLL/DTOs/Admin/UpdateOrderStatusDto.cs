using System.ComponentModel.DataAnnotations;

namespace GalleryVelvet.BLL.DTOs.Admin;

public sealed class UpdateOrderStatusDto
{
    [Required(ErrorMessage = "Статус заказа обязателен")]
    public required Guid StatusId { get; set; }
}