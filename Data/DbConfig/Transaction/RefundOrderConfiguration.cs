using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.DbConfig.Transaction;

public class RefundOrderConfiguration : IEntityTypeConfiguration<RefundOrder>
{
    public void Configure(EntityTypeBuilder<RefundOrder> entity)
    {
        entity.HasKey(e => e.RefundOrderId).HasName("refund_order_pkey");

        entity.ToTable("refund_order");

        entity.HasIndex(e => e.OrderId, "IX_refund_order_order_id");

        entity.Property(e => e.RefundOrderId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("refund_order_id");
        entity.Property(e => e.Adress).HasColumnName("adress");
        entity.Property(e => e.AmountOther)
            .HasPrecision(10, 2)
            .HasColumnName("amount_other");
        entity.Property(e => e.Code)
            .HasMaxLength(100)
            .HasColumnName("code");
        entity.Property(e => e.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("create_at");
        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasColumnName("create_by");
        entity.Property(e => e.ModifiedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("modify_at");
        entity.Property(e => e.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("modify_by");
        entity.Property(e => e.Note).HasColumnName("note");
        entity.Property(e => e.OrderId).HasColumnName("order_id");
        entity.Property(e => e.Quantity).HasColumnName("quantity");
        entity.Property(e => e.Status).HasColumnName("status");
        entity.Property(e => e.TotalAmount)
            .HasPrecision(10, 2)
            .HasColumnName("total_amount");

        entity.HasOne(d => d.Order).WithMany(p => p.RefundOrders)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("refund_order_order_id_fkey");
    }
}