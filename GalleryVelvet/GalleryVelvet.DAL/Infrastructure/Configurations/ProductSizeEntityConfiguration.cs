using GalleryVelvet.Domain.M2M;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalleryVelvet.DAL.Infrastructure.Configurations;

public sealed class ProductSizeEntityConfiguration : IEntityTypeConfiguration<ProductSizeEntity>
{
    public void Configure(EntityTypeBuilder<ProductSizeEntity> builder)
    {
        builder.ToTable(nameof(AppDbContext.ProductSizes));

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.ProductId, x.SizeId }).IsUnique();

        builder
            .HasOne(x => x.Product)
            .WithMany(p => p.ProductSizes)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Size)
            .WithMany(s => s.ProductSizes)
            .HasForeignKey(x => x.SizeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
