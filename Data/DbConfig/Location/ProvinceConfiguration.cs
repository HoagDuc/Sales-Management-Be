using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.LocationEntity;

namespace ptdn_net.Data.DbConfig.Location;

public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> entity)
    {
        entity.HasKey(e => e.ProvinceId).HasName("province_pkey");

        entity.ToTable("province");

        entity.Property(e => e.ProvinceId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("province_id");
        entity.Property(e => e.Code)
            .HasMaxLength(10)
            .HasColumnName("code");
        entity.Property(e => e.IsActive)
            .HasComment("True: Active. False: Inactive")
            .HasColumnName("is_active");
        entity.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name");
    }
}