using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.DbConfig.Product;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Entity.ProductEntity.Category> entity)
    {
        entity.HasKey(e => e.CategoryId).HasName("category_pkey");

        entity.ToTable("category");

        entity.Property(e => e.CategoryId)
            .UseIdentityAlwaysColumn()
            .HasColumnName("category_id");
        entity.Property(e => e.Code)
            .HasMaxLength(100)
            .HasColumnName("code");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");
    }
}