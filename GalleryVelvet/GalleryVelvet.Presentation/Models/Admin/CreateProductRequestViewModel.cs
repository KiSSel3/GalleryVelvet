using System.ComponentModel.DataAnnotations;

namespace GalleryVelvet.Presentation.Models.Admin;

public class CreateProductRequestViewModel
{
    [Required(ErrorMessage = "Название продукта обязательно")]
    [StringLength(200, ErrorMessage = "Название не может быть длиннее 200 символов")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Описание не может быть длиннее 1000 символов")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Цена обязательна")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
    public decimal Price { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Цена со скидкой не может быть отрицательной")]
    public decimal? DiscountPrice { get; set; }

    [Required(ErrorMessage = "Категория обязательна")]
    public Guid CategoryId { get; set; }

    public List<Guid>? TagIds { get; set; }
    public List<Guid>? SizeIds { get; set; }
    
    public IFormFile[]? Images { get; set; }
}