using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.DbConfig.Transaction;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> entity)
    {
        entity.HasKey(e => new { e.ProductId, e.OrderId }).HasName("order_detail_pkey");

        entity.ToTable("order_detail");

        entity.Property(e => e.ProductId).HasColumnName("product_id");
        entity.Property(e => e.OrderId).HasColumnName("order_id");
        entity.Property(e => e.Discount).HasColumnName("discount");
        entity.Property(e => e.Price)
            .HasPrecision(10, 2)
            .HasColumnName("price");
        entity.Property(e => e.Quantity).HasColumnName("quantity");

        entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("order_detail_order_id_fkey");

        entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("order_detail_product_id_fkey");
    }
}