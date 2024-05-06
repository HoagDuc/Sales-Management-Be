using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ptdn_net.Data.DbConfig.System;

public class PermissionConfiguration : IEntityTypeConfiguration<Entity.SystemEntity.Permission>
{
    public void Configure(EntityTypeBuilder<Entity.SystemEntity.Permission> entity)
    {
        entity.HasKey(e => e.PermissionId).HasName("permission_pkey");

        entity.ToTable("permission");

        entity.Property(e => e.PermissionId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("permission_id");
        entity.Property(e => e.Code)
            .HasMaxLength(100)
            .HasColumnName("code");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");
    }
}