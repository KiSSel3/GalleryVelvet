using GalleryVelvet.Domain.M2M;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalleryVelvet.DAL.Infrastructure.Configurations;

public sealed class ProductTagEntityConfiguration : IEntityTypeConfiguration<ProductTagEntity>
{
    public void Configure(EntityTypeBuilder<ProductTagEntity> builder)
    {
        builder.ToTable(nameof(AppDbContext.ProductTags));

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.ProductId, x.TagId }).IsUnique();

        builder
            .HasOne(x => x.Product)
            .WithMany(p => p.ProductTags)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Tag)
            .WithMany(t => t.ProductTags)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
