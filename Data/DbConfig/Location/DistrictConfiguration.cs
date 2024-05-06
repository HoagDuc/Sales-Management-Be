using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.LocationEntity;

namespace ptdn_net.Data.DbConfig.Location;

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> entity)
    {
        entity.HasKey(e => e.DistrictId).HasName("district_pkey");

        entity.ToTable("district");

        entity.Property(e => e.DistrictId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("district_id");
        entity.Property(e => e.Code)
            .HasMaxLength(10)
            .HasColumnName("code");
        entity.Property(e => e.IsActive).HasColumnName("is_active");
        entity.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name");
        entity.Property(e => e.ProvinceCode)
            .HasMaxLength(10)
            .HasColumnName("province_code");
    }
}