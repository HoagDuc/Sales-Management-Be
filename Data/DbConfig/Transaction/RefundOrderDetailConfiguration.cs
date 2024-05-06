using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.DbConfig.Transaction;

public class RefundOrderDetailConfiguration : IEntityTypeConfiguration<RefundOrderDetail>
{
    public void Configure(EntityTypeBuilder<RefundOrderDetail> entity)
    {
        entity.HasKey(e => new { e.ProductId, e.RefundOrderId }).HasName("refund_order_detail_pkey");

        entity.ToTable("refund_order_detail");

        entity.HasIndex(e => e.RefundOrderId, "IX_refund_order_detail_refund_order_id");

        entity.Property(e => e.ProductId).HasColumnName("product_id");
        entity.Property(e => e.RefundOrderId)
            .HasComment("map với bảng refund_order")
            .HasColumnName("refund_order_id");
        entity.Property(e => e.Note).HasColumnName("note");
        entity.Property(e => e.Price)
            .HasPrecision(10, 2)
            .HasColumnName("price");
        entity.Property(e => e.Quantity).HasColumnName("quantity");

        entity.HasOne(d => d.Product).WithMany(p => p.RefundOrderDetails)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("refund_order_detail_product_id_fkey");

        entity.HasOne(d => d.RefundOrder).WithMany(p => p.RefundOrderDetails)
            .HasForeignKey(d => d.RefundOrderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("refund_order_detail_refund_order_id_fkey");
    }
}