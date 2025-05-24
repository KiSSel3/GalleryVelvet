using GalleryVelvet.DAL.Infrastructure;
using GalleryVelvet.DAL.Repositories.Interfaces;
using GalleryVelvet.Domain.Entities;

namespace GalleryVelvet.DAL.Repositories.Implementations;

public sealed class TagRepository(AppDbContext dbContext)
    : BaseRepository<TagEntity>(dbContext), ITagRepository;