using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.DbConfig.Transaction;

public class TransactionTypeConfiuration : IEntityTypeConfiguration<TransactionType>
{
    public void Configure(EntityTypeBuilder<TransactionType> entity)
    {
        entity.HasKey(e => e.TransactionTypeId).HasName("transaction_type_pkey");

        entity.ToTable("transaction_type");

        entity.Property(e => e.TransactionTypeId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("transaction_type_id");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");
    }
}