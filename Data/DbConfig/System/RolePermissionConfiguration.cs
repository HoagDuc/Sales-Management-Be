using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Data.DbConfig.System;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> entity)
    {
        entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("oct_role_authority_pkey");

        entity.ToTable("oct_role_authority");

        entity.Property(e => e.RoleId)
            .HasColumnName("role_id");

        entity.Property(e => e.PermissionId)
            .HasColumnName("permission_id");
    }
}