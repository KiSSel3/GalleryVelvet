using System.ComponentModel.DataAnnotations;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Admin;

public sealed class EditProductRequestViewModel
{
    [Required(ErrorMessage = "Название продукта обязательно")]
    [StringLength(200, ErrorMessage = "Название не может быть длиннее 200 символов")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Описание не может быть длиннее 1000 символов")]
    public string? Description { get; set; }

    [StringLength(1000, ErrorMessage = "Состав и уход не может быть длиннее 1000 символов")]
    public string? CompositionAndCare { get; set; }

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
    
    public List<ExistingImageViewModel>? ExistingImages { get; set; }
    public List<Guid>? ImagesToDelete { get; set; }
}