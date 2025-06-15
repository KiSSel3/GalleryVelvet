using System.ComponentModel.DataAnnotations;

namespace GalleryVelvet.BLL.DTOs.Admin;

public sealed class CreateProductDto
{
    [Required(ErrorMessage = "Название продукта обязательно")]
    [StringLength(200, ErrorMessage = "Название не может быть длиннее 200 символов")]
    public required string Name { get; set; }

    [StringLength(1000, ErrorMessage = "Описание не может быть длиннее 1000 символов")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Цена обязательна")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
    public required decimal Price { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Цена со скидкой не может быть отрицательной")]
    public decimal? DiscountPrice { get; set; }

    [Required(ErrorMessage = "Категория обязательна")]
    public required Guid CategoryId { get; set; }

    public List<Guid> TagIds { get; set; } = [];
    public List<Guid> SizeIds { get; set; } = [];
    public List<ProductImageDto> Images { get; set; } = [];
}