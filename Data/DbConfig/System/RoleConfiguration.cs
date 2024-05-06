using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Data.DbConfig.System;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> entity)
    {
        entity.HasKey(e => e.RoleId).HasName("role_pkey");

        entity.ToTable("role");

        entity.Property(e => e.RoleId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("role_id");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.IsActive).HasColumnName("isactive");
        entity.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name");

        entity.HasMany(d => d.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermission>(
                l => l.HasOne<Permission>()
                    .WithMany(e => e.RolePermissions)
                    .HasForeignKey(e => e.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_roau_permission_id"),
                r => r.HasOne<Role>()
                    .WithMany(e => e.RolePermissions)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_roau_role_id")
            );
    }
}