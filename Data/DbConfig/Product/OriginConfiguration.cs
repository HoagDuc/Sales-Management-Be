using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.DbConfig.Product;

public class OriginConfiguration : IEntityTypeConfiguration<Origin>
{
    public void Configure(EntityTypeBuilder<Origin> entity)
    {
        entity.HasKey(e => e.OriginId).HasName("origin_pkey");

        entity.ToTable("origin");

        entity.Property(e => e.OriginId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("origin_id");
        entity.Property(e => e.Code)
            .HasMaxLength(100)
            .HasColumnName("code");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");
    }
}