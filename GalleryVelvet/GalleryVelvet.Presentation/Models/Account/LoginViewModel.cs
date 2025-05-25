using System.ComponentModel.DataAnnotations;

namespace GalleryVelvet.Presentation.Models.Account;

public sealed class LoginViewModel
{
    [Required(ErrorMessage = "Логин обязателен")]
    [StringLength(100, ErrorMessage = "Логин не должен превышать 100 символов")]
    public required string Login { get; set; }
    
    [Required(ErrorMessage = "Пароль обязателен")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Пароль должен содержать от 4 до 100 символов")]
    public required string Password { get; set; }
}