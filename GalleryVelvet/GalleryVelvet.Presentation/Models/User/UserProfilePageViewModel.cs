namespace GalleryVelvet.Presentation.Models.User;

public sealed class UserProfilePageViewModel
{
    public ProfileViewModel Profile { get; set; } = null!;
    public IEnumerable<OrderHistoryViewModel> Orders { get; set; } = [];
    public string ActiveTab { get; set; } = "profile";
}