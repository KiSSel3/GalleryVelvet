namespace GalleryVelvet.Presentation.Models.Category;

public sealed class CategoryToSidebarViewModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    
    public int ProductCount { get; set; }
}