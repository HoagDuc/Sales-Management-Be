using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.DbConfig.Product;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> entity)
    {
        entity.HasKey(e => e.UnitId).HasName("unit_pkey");

        entity.ToTable("unit");

        entity.Property(e => e.UnitId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("unit_id");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");
    }
}