using GalleryVelvet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalleryVelvet.DAL.Infrastructure.Configurations;

public sealed class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.ToTable(nameof(AppDbContext.OrderItems));

        builder.HasKey(item => item.Id);

        builder
            .HasOne(item => item.Product)
            .WithMany(product => product.OrderItems)
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(item => item.Size)
            .WithMany(size => size.OrderItems)
            .HasForeignKey(item => item.SizeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(item => item.Order)
            .WithMany(order => order.OrderItems)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
