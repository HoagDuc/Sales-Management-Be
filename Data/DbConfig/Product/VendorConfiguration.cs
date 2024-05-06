using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.DbConfig.Product;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> entity)
    {
        entity.HasKey(e => e.VendorId).HasName("vendor_pkey");

        entity.ToTable("vendor");

        entity.Property(e => e.VendorId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("vendor_id");
        entity.Property(e => e.Address).HasColumnName("address");
        entity.Property(e => e.Code)
            .HasMaxLength(100)
            .HasColumnName("code");
        entity.Property(e => e.Debt)
            .HasPrecision(10, 2)
            .HasColumnName("debt");
        entity.Property(e => e.DistrictId).HasColumnName("district_id");
        entity.Property(e => e.Email)
            .HasMaxLength(150)
            .HasColumnName("email");
        entity.Property(e => e.Fax)
            .HasPrecision(10, 2)
            .HasColumnName("fax");
        entity.Property(e => e.Name)
            .HasMaxLength(150)
            .HasColumnName("name");
        entity.Property(e => e.Phone)
            .HasMaxLength(15)
            .HasColumnName("phone");
        entity.Property(e => e.ProvinceId).HasColumnName("province_id");
        entity.Property(e => e.SubDistrictId).HasColumnName("subdistrict_id");
        entity.Property(e => e.Tax)
            .HasPrecision(10, 2)
            .HasColumnName("tax");
        entity.Property(e => e.Website).HasColumnName("website");
    }
}