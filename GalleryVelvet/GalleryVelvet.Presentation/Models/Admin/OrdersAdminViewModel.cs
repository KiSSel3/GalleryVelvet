using GalleryVelvet.BLL.RequestFeatures;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.Presentation.Models.Admin;

public sealed class OrdersAdminViewModel
{
    public PagedList<OrderEntity> Orders { get; set; } = null!;
    public IEnumerable<OrderStatusEntity> OrderStatuses { get; set; } = [];
    public string? SearchTerm { get; set; }
    public Guid? SelectedStatusId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}