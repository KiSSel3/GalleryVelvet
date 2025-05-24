namespace GalleryVelvet.BLL.DTOs.Authorization;

public sealed class RegisterDto
{
    public required string Login { get; set; }
    
    public required string Password { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string? Email { get; set; }
    
    public string? PhoneNumber { get; set; }
}