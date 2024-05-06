using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ptdn_net.Data.DbConfig.Product;

public class BrandConfiguration : IEntityTypeConfiguration<Entity.ProductEntity.Brand>
{
    public void Configure(EntityTypeBuilder<Entity.ProductEntity.Brand> entity)
    {
        entity.HasKey(e => e.BrandId).HasName("brand_pkey");

        entity.ToTable("brand");

        entity.Property(e => e.BrandId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("brand_id");
        entity.Property(e => e.Code)
            .HasMaxLength(100)
            .HasColumnName("code");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");
    }
}