using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.DbConfig.Product;

public class CapitailPriceVersionConfiguration : IEntityTypeConfiguration<CapitailPriceVersion>
{
    public void Configure(EntityTypeBuilder<CapitailPriceVersion> entity)
    {
        entity.HasKey(e => e.CapitalPriceId).HasName("capitail_price_version_pkey");

        entity.ToTable("capitail_price_version");

        entity.Property(e => e.CapitalPriceId).HasColumnName("capital_price_id");
        entity.Property(e => e.CapitalPrice)
            .HasPrecision(10, 2)
            .HasColumnName("capital_price");
        entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
        entity.Property(e => e.Quantity).HasColumnName("quantity");
        entity.Property(e => e.ReceiptDate)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("receipt_date");

        entity.HasOne(d => d.Inventory).WithMany(p => p.CapitailPriceVersions)
            .HasForeignKey(d => d.InventoryId)
            .HasConstraintName("capitail_price_version_inventory_id_fkey");
    }
}