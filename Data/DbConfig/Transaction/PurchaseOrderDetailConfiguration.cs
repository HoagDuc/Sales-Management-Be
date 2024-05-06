using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.DbConfig.Transaction;

public class PurchaseOrderDetailConfiguration : IEntityTypeConfiguration<PurchaseOrderDetail>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderDetail> entity)
    {
        entity.HasKey(e => new { e.PurchaseOrderId, e.ProductId }).HasName("purchase_order_detail_pkey");

        entity.ToTable("purchase_order_detail");

        entity.Property(e => e.PurchaseOrderId).HasColumnName("purchase_order_id");
        entity.Property(e => e.ProductId).HasColumnName("product_id");
        entity.Property(e => e.Discount).HasColumnName("discount");
        entity.Property(e => e.Price)
            .HasPrecision(10, 2)
            .HasColumnName("price");
        entity.Property(e => e.Quantity).HasColumnName("quantity");

        entity.HasOne(d => d.Product).WithMany(p => p.PurchaseOrderDetails)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("purchase_order_detail_product_id_fkey");

        entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderDetails)
            .HasForeignKey(d => d.PurchaseOrderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("purchase_order_detail_purchase_order_id_fkey");
    }
}