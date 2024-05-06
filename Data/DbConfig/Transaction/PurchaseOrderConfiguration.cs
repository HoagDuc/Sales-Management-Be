using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.DbConfig.Transaction;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> entity)
    {
        entity.HasKey(e => e.PurchaseOrderId).HasName("purchase_order_pkey");

            entity.ToTable("purchase_order");

            entity.HasIndex(e => e.VendorId, "IX_purchase_order_vendor_id");

            entity.Property(e => e.PurchaseOrderId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("purchase_order_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.AmountDue)
                .HasPrecision(10, 2)
                .HasColumnName("amount_due");
            entity.Property(e => e.AmountOther)
                .HasPrecision(10, 2)
                .HasColumnName("amount_other");
            entity.Property(e => e.AmountPaid)
                .HasPrecision(10, 2)
                .HasColumnName("amount_paid");
            entity.Property(e => e.AmountRemaining)
                .HasPrecision(10, 2)
                .HasColumnName("amount_remaining");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("create_by");
            entity.Property(e => e.DeliveryDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("delivery_date");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modify_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(100)
                .HasColumnName("modify_by");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.OrderDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("order_date");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Tax)
                .HasPrecision(10, 2)
                .HasColumnName("tax");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount");
            entity.Property(e => e.Vat)
                .HasPrecision(10, 2)
                .HasColumnName("vat");
            entity.Property(e => e.VendorId).HasColumnName("vendor_id");

            entity.HasOne(d => d.Vendor).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("purchase_order_vendor_id_fkey");
    }
}