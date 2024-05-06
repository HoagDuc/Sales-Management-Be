using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ptdn_net.Data.DbConfig.Product;

public class ProductConfiguration : IEntityTypeConfiguration<Entity.ProductEntity.Product>
{
    public void Configure(EntityTypeBuilder<Entity.ProductEntity.Product> entity)
    {
         entity.HasKey(e => e.ProductId).HasName("product_pkey");

            entity.ToTable("product");

            entity.HasIndex(e => e.BrandId, "IX_product_brand_id");

            entity.HasIndex(e => e.CategoryId, "IX_product_category_id");

            entity.HasIndex(e => e.OriginId, "IX_product_origin_id");

            entity.HasIndex(e => e.UnitId, "IX_product_unit_id");

            entity.HasIndex(e => e.VendorId, "IX_product_vendor_id");

            entity.Property(e => e.ProductId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("product_id");
            entity.Property(e => e.Barcode)
                .HasMaxLength(100)
                .HasColumnName("barcode");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.ChietKhauNhap)
                .HasPrecision(10, 2)
                .HasColumnName("chiet_khau_nhap");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.CostPrice)
                .HasPrecision(10, 2)
                .HasColumnName("cost_price");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.GiaNiemYet)
                .HasPrecision(10, 2)
                .HasColumnName("gia_niem_yet");
            entity.Property(e => e.IsActive).HasColumnName("isactive");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.OriginId).HasColumnName("origin_id");
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(255)
                .HasColumnName("short_description");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.Vat)
                .HasPrecision(10, 2)
                .HasColumnName("vat");
            entity.Property(e => e.VendorId).HasColumnName("vendor_id");
            entity.Property(e => e.Volume).HasColumnName("volume");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("product_brand_id_fkey");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_category_id_fkey");

            entity.HasOne(d => d.Origin).WithMany(p => p.Products)
                .HasForeignKey(d => d.OriginId)
                .HasConstraintName("product_origin_id_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.Products)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_unit_id_fkey");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Products)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("product_vendor_id_fkey");
    }
}