using System.ComponentModel.DataAnnotations;

namespace GalleryVelvet.BLL.DTOs.Order;

public sealed class CreateOrderDto
{
    [Required(ErrorMessage = "Имя обязательно для заполнения")]
    [StringLength(100, ErrorMessage = "Имя не может быть длиннее 100 символов")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Фамилия обязательна для заполнения")]
    [StringLength(100, ErrorMessage = "Фамилия не может быть длиннее 100 символов")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email обязателен для заполнения")]
    [EmailAddress(ErrorMessage = "Неверный формат email")]
    [StringLength(255, ErrorMessage = "Email не может быть длиннее 255 символов")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Номер телефона обязателен для заполнения")]
    [Phone(ErrorMessage = "Неверный формат номера телефона")]
    [StringLength(20, ErrorMessage = "Номер телефона не может быть длиннее 20 символов")]
    public required string PhoneNumber { get; set; }

    [StringLength(500, ErrorMessage = "Комментарий не может быть длиннее 500 символов")]
    public string? Comments { get; set; }

    [Required(ErrorMessage = "Тип доставки обязателен для выбора")]
    public required string DeliveryType { get; set; }
}