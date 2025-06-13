using System.ComponentModel.DataAnnotations;

namespace GalleryVelvet.Presentation.Models.User;

public sealed class ProfileViewModel
{
    [Required(ErrorMessage = "Логин обязателен")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Логин должен содержать от 3 до 50 символов")]
    [Display(Name = "Логин")]
    public required string Login { get; set; }

    [StringLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
    [Display(Name = "Имя")]
    public string? FirstName { get; set; }

    [StringLength(100, ErrorMessage = "Фамилия не должна превышать 100 символов")]
    [Display(Name = "Фамилия")]
    public string? LastName { get; set; }

    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    [StringLength(255, ErrorMessage = "Email не должен превышать 255 символов")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Некорректный формат номера телефона")]
    [StringLength(20, ErrorMessage = "Номер телефона не должен превышать 20 символов")]
    [Display(Name = "Номер телефона")]
    public string? PhoneNumber { get; set; }
}