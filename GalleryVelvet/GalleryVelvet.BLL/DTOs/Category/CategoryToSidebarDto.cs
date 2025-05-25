namespace GalleryVelvet.BLL.DTOs.Category;

public sealed class CategoryToSidebarDto
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public int ProductCount { get; set; }
}