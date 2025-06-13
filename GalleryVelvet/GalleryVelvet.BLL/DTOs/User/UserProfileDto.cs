namespace GalleryVelvet.BLL.DTOs.User;

public sealed class UserProfileDto
{
    public Guid Id { get; set; }
    public required string Login { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}