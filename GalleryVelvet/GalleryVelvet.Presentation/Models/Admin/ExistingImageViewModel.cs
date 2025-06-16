namespace GalleryVelvet.Presentation.Models.Admin;

public sealed class ExistingImageViewModel
{
    public Guid Id { get; set; }
    public string Image { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
}