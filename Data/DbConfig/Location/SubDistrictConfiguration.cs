using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.LocationEntity;

namespace ptdn_net.Data.DbConfig.Location;

public class SubDistrictConfiguration : IEntityTypeConfiguration<Subdistrict>
{
    public void Configure(EntityTypeBuilder<Subdistrict> entity)
    {
        entity.HasKey(e => e.SubDistrictId).HasName("subdistrict_pkey");

        entity.ToTable("subdistrict");

        entity.Property(e => e.SubDistrictId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("subdistrict_id");
        entity.Property(e => e.Code)
            .HasMaxLength(10)
            .HasColumnName("code");
        entity.Property(e => e.DistrictCode)
            .HasMaxLength(10)
            .HasComment("Map bảng district trường code")
            .HasColumnName("district_code");
        entity.Property(e => e.IsActive)
            .HasComment("True: Active. False: Inactive")
            .HasColumnName("is_active");
        entity.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name");
        entity.Property(e => e.ProvinceCode)
            .HasMaxLength(10)
            .HasComment("Map bảng province trường code")
            .HasColumnName("province_code");
        entity.Property(e => e.ShortName)
            .HasMaxLength(100)
            .HasColumnName("short_name");
    }
}