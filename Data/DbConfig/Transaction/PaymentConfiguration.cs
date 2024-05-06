using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.DbConfig.Transaction;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> entity)
    {
        entity.HasKey(e => e.PaymentId).HasName("payment_pkey");

        entity.ToTable("payment");

        entity.Property(e => e.PaymentId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("payment_id");
        entity.Property(e => e.CreateAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("create_at");
        entity.Property(e => e.CreateBy)
            .HasMaxLength(100)
            .HasColumnName("create_by");
        entity.Property(e => e.DeliveryMethod).HasColumnName("delivery_method");
        entity.Property(e => e.PaymentAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("payment_at");
        entity.Property(e => e.PaymentTotal)
            .HasPrecision(10, 2)
            .HasColumnName("payment_total");
        entity.Property(e => e.Type).HasColumnName("type");
    }
}