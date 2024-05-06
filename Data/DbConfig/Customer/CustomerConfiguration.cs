using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ptdn_net.Data.DbConfig.Customer;

public class CustomerConfiguration : IEntityTypeConfiguration<Entity.CustomerEntity.Customer>
{
    public void Configure(EntityTypeBuilder<Entity.CustomerEntity.Customer> entity)
    {
        entity.HasKey(e => e.CustomerId).HasName("customer_pkey");

        entity.ToTable("customer");

        entity.Property(e => e.CustomerId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("customer_id");
        entity.Property(e => e.Address).HasColumnName("address");
        entity.Property(e => e.Code)
            .HasMaxLength(100)
            .HasColumnName("code");
        entity.Property(e => e.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("create_at");
        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasColumnName("create_by");
        entity.Property(e => e.CustomerGroupId).HasColumnName("customer_group_id");
        entity.Property(e => e.Debt)
            .HasPrecision(10, 2)
            .HasColumnName("debt");
        entity.Property(e => e.DistrictId).HasColumnName("district_id");
        entity.Property(e => e.Dob)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("dob");
        entity.Property(e => e.Email)
            .HasMaxLength(150)
            .HasColumnName("email");
        entity.Property(e => e.Fax)
            .HasPrecision(10, 2)
            .HasColumnName("fax");
        entity.Property(e => e.Gender).HasColumnName("gender");
        entity.Property(e => e.ModifiedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("modify_at");
        entity.Property(e => e.ModifiedBy)
            .HasMaxLength(100)
            .HasColumnName("modify_by");
        entity.Property(e => e.Name)
            .HasMaxLength(150)
            .HasColumnName("name");
        entity.Property(e => e.Phone)
            .HasMaxLength(15)
            .HasColumnName("phone");
        entity.Property(e => e.ProvinceId).HasColumnName("province_id");
        entity.Property(e => e.SubDistrictId).HasColumnName("subdistrict_id");
        entity.Property(e => e.Tax).HasColumnName("tax");
        entity.Property(e => e.TotalExpenditure)
            .HasPrecision(10, 2)
            .HasColumnName("total_expenditure");
        entity.Property(e => e.Website).HasColumnName("website");

        entity.HasOne(d => d.CustomerGroup).WithMany(p => p.Customers)
            .HasForeignKey(d => d.CustomerGroupId)
            .HasConstraintName("customer_customer_group_id_fkey");
    }
}