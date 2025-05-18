using System.Reflection;
using GalleryVelvet.Domain.Entities;
using GalleryVelvet.Domain.M2M;
using Microsoft.EntityFrameworkCore;

namespace GalleryVelvet.DAL.Infrastructure;

public sealed class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<UserRoleEntity> UserRoles { get; set; }
    
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<TagEntity> Tags { get; set; }
    public DbSet<SizeEntity> Sizes { get; set; }
    public DbSet<ImageEntity> Images { get; set; }
    public DbSet<ProductTagEntity> ProductTags { get; set; }
    public DbSet<ProductSizeEntity> ProductSizes { get; set; }
    
    public DbSet<CartItemEntity> CartItems { get; set; }

    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<OrderStatusEntity> OrderStatuses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.Migrate();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}