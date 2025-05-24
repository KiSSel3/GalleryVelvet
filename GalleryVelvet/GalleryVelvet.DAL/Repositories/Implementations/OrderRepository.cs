using GalleryVelvet.DAL.Infrastructure;
using GalleryVelvet.DAL.Repositories.Interfaces;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.DAL.Repositories.Implementations;

public sealed class OrderRepository(AppDbContext dbContext)
    : BaseRepository<OrderEntity>(dbContext), IOrderRepository;