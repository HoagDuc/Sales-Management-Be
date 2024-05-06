using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.CustomerEntity;

namespace ptdn_net.Data.DbConfig.Customer;

public class CustomerGroupConfiguration : IEntityTypeConfiguration<CustomerGroup>
{
    public void Configure(EntityTypeBuilder<CustomerGroup> entity)
    {
        entity.HasKey(e => e.CustomerGroupId).HasName("customer_group_pkey");

        entity.ToTable("customer_group");

        entity.Property(e => e.CustomerGroupId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("customer_group_id");
        entity.Property(e => e.Code)
            .HasMaxLength(100)
            .HasColumnName("code");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.Discount).HasColumnName("discount");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");
    }
}