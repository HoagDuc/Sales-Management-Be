using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ptdn_net.Data.DbConfig.Transaction;

public class TransactionConfiguration : IEntityTypeConfiguration<Entity.TransactionEntity.Transaction>
{
    public void Configure(EntityTypeBuilder<Entity.TransactionEntity.Transaction> entity)
    {
        entity.HasKey(e => e.TransactionId).HasName("transaction_pkey");

        entity.ToTable("transaction");

        entity.Property(e => e.TransactionId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("transaction_id");
        entity.Property(e => e.Code)
            .HasMaxLength(100)
            .HasColumnName("code");
        entity.Property(e => e.Date)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("date");
        entity.Property(e => e.Price)
            .HasPrecision(10, 2)
            .HasColumnName("price");
        entity.Property(e => e.TransactionTypeId).HasColumnName("transaction_type_id");

        entity.HasOne(d => d.TransactionType).WithMany(p => p.Transactions)
            .HasForeignKey(d => d.TransactionTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("transaction_transaction_type_id_fkey");
    }
}