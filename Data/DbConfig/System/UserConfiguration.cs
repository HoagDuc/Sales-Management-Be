using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Data.DbConfig.System;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(e => e.UserId).HasName("user_pkey");

        entity.ToTable("user");

        entity.Property(e => e.UserId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("user_id");
        entity.Property(e => e.Address).HasColumnName("address");
        entity.Property(e => e.Avatar).HasColumnName("avatar");
        entity.Property(e => e.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("create_at");
        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasColumnName("create_by");
        entity.Property(e => e.Dob)
            .HasColumnType("timestamp(6) without time zone")
            .HasColumnName("dob");
        entity.Property(e => e.Email)
            .HasMaxLength(150)
            .HasColumnName("email");
        entity.Property(e => e.Fullname)
            .HasMaxLength(255)
            .HasColumnName("fullname");
        entity.Property(e => e.Gender)
            .HasComment("0 = Nam. 1 = Nữ. 2 = Etc.")
            .HasColumnName("gender");
        entity.Property(e => e.IsActive)
            .HasComment("t = Active. f = InActive.")
            .HasColumnName("isactive");
        entity.Property(e => e.ModifiedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("modified_at");
        entity.Property(e => e.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("modified_by");
        entity.Property(e => e.Password)
            .HasMaxLength(255)
            .HasComment("Password mã hoá theo Bcrypt")
            .HasColumnName("password");
        entity.Property(e => e.Phone)
            .HasMaxLength(15)
            .HasColumnName("phone");
        entity.Property(e => e.RoleId).HasColumnName("role_id");
        entity.Property(e => e.Username)
            .HasMaxLength(100)
            .HasColumnName("username");
    }
}