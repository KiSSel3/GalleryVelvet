using GalleryVelvet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalleryVelvet.DAL.Infrastructure.Configurations;

public sealed class CartItemEntityConfiguration : IEntityTypeConfiguration<CartItemEntity>
{
    public void Configure(EntityTypeBuilder<CartItemEntity> builder)
    {
        builder.ToTable(nameof(AppDbContext.CartItems));
        
        builder.HasKey(cartItem => cartItem.Id);

        builder
            .HasOne(cartItem => cartItem.User)
            .WithMany()
            .HasForeignKey(carItem => carItem.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(carItem => carItem.Product)
            .WithMany(product => product.CartItems)
            .HasForeignKey(carItem => carItem.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(cartItem => cartItem.Size)
            .WithMany(size => size.CartItems)
            .HasForeignKey(cartItem => cartItem.SizeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}