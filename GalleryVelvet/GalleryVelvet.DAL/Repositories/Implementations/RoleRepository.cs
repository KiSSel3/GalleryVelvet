using GalleryVelvet.DAL.Infrastructure;
using GalleryVelvet.DAL.Repositories.Interfaces;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.DAL.Repositories.Implementations;

public sealed class RoleRepository(AppDbContext dbContext)
    : BaseRepository<RoleEntity>(dbContext), IRoleRepository;