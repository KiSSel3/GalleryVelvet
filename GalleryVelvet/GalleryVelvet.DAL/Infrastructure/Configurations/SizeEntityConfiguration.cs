using GalleryVelvet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalleryVelvet.DAL.Infrastructure.Configurations;

public sealed class SizeEntityConfiguration : IEntityTypeConfiguration<SizeEntity>
{
    public void Configure(EntityTypeBuilder<SizeEntity> builder)
    {
        builder.ToTable(nameof(AppDbContext.Sizes));

        builder.HasKey(size => size.Id);

        builder
            .HasMany(size => size.OrderItems)
            .WithOne(oi => oi.Size)
            .HasForeignKey(oi => oi.SizeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(size => size.CartItems)
            .WithOne(ci => ci.Size)
            .HasForeignKey(ci => ci.SizeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
