using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.DbConfig.Transaction;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> entity)
    {
        entity.HasKey(e => e.OrderId).HasName("order_pkey");

        entity.ToTable("order");

        entity.Property(e => e.OrderId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("order_id");
        entity.Property(e => e.Address).HasColumnName("address");
        entity.Property(e => e.AmountDue)
            .HasPrecision(10, 2)
            .HasColumnName("amount_due");
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
            .HasColumnType("timestamp without time zone")
            .HasColumnName("create_at");
        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasColumnName("create_by");
        entity.Property(e => e.CustomerId).HasColumnName("customer_id");
        entity.Property(e => e.DeliveryMethod).HasColumnName("delivery_method");
        entity.Property(e => e.Discount).HasColumnName("discount");
        entity.Property(e => e.ModifiedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("modify_at");
        entity.Property(e => e.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("modify_by");
        entity.Property(e => e.Note).HasColumnName("note");
        entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
        entity.Property(e => e.Phone)
            .HasMaxLength(15)
            .HasColumnName("phone");
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
        entity.Property(e => e.TotalNhap)
            .HasPrecision(10, 2)
            .HasColumnName("total_nhap");
        entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("order_customer_id_fkey");
    }
}