using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity;

namespace ptdn_net.Data.DbConfig;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> entity)
    {
        entity.HasKey(e => e.InventoryId).HasName("inventory_pkey");

        entity.ToTable("inventory");

        entity.HasIndex(e => e.ProductId, "IX_inventory_product_id");

        entity.Property(e => e.InventoryId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("inventory_id");
        entity.Property(e => e.CapitalPrice)
            .HasPrecision(10, 2)
            .HasColumnName("capital_price");
        entity.Property(e => e.DispatchDate)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("dispatch_date");
        entity.Property(e => e.MinQuantity).HasColumnName("min_quantity");
        entity.Property(e => e.ProductId).HasColumnName("product_id");
        entity.Property(e => e.Quantity).HasColumnName("quantity");
        entity.Property(e => e.ReceiptDate)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("receipt_date");

        entity.HasOne(d => d.Product).WithMany(p => p.Inventories).HasForeignKey(d => d.ProductId);
    }
}