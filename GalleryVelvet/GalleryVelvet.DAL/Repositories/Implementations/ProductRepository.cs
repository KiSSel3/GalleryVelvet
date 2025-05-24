using GalleryVelvet.DAL.Infrastructure;
using GalleryVelvet.DAL.Repositories.Interfaces;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.DAL.Repositories.Implementations;

public sealed class ProductRepository(AppDbContext dbContext)
    : BaseRepository<ProductEntity>(dbContext), IProductRepository;