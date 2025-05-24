namespace GalleryVelvet.BLL.DTOs.Authorization;

public sealed class LoginDto
{
    public required string Login { get; set; }
    
    public required string Password { get; set; }
}