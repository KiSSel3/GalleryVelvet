using GalleryVelvet.DAL.Infrastructure;
using GalleryVelvet.DAL.Repositories.Interfaces;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.DAL.Repositories.Implementations;

public sealed class OrderStatusRepository(AppDbContext dbContext)
    : BaseRepository<OrderStatusEntity>(dbContext), IOrderStatusRepository;