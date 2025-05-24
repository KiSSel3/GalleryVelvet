using System.ComponentModel.DataAnnotations;

namespace GalleryVelvet.Presentation.Models.Account;

public sealed class RegisterViewModel
{
    [Required(ErrorMessage = "Логин обязателен")]
    [StringLength(100, ErrorMessage = "Логин не должен превышать 100 символов")]
    public required string Login { get; set; }
    
    [Required(ErrorMessage = "Пароль обязателен")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Пароль должен содержать от 4 до 100 символов")]
    public required string Password { get; set; }
    
    [StringLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
    public string? FirstName { get; set; }
    
    [StringLength(100, ErrorMessage = "Фамилия не должна превышать 100 символов")]
    public string? LastName { get; set; }
    
    [EmailAddress(ErrorMessage = "Неверный формат Email")]
    [StringLength(150)]
    public string? Email { get; set; }
    
    [Phone(ErrorMessage = "Неверный формат номера телефона")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
}