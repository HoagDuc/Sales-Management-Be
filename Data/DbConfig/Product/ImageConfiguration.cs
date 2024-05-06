using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ptdn_net.Data.DbConfig.Product;

public class ImageConfiguration : IEntityTypeConfiguration<Entity.ProductEntity.Image>
{
    public void Configure(EntityTypeBuilder<Entity.ProductEntity.Image> entity)
    {
        entity.HasKey(e => e.ImageId).HasName("image_pkey");

        entity.ToTable("image");

        entity.Property(e => e.ImageId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("image_id");
        entity.Property(e => e.FileId).HasColumnName("file_id");
        entity.Property(e => e.ProductId).HasColumnName("product_id");

        entity.HasOne(d => d.File).WithMany(p => p.Images)
            .HasForeignKey(d => d.FileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("image_file_id_fkey");

        entity.HasOne(d => d.Product).WithMany(p => p.Images)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("image_product_id_fkey");
    }
}