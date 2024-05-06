using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = ptdn_net.Data.Entity.ProductEntity.File;

namespace ptdn_net.Data.DbConfig.System;

public class FileConfiguration
{
    public void Configure(EntityTypeBuilder<File> entity)
    {
        entity.HasKey(e => e.FileId).HasName("file_pkey");

        entity.ToTable("file");

        entity.Property(e => e.FileId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("file_id");
        entity.Property(e => e.ContentSize).HasColumnName("content_size");
        entity.Property(e => e.ContentType)
            .HasMaxLength(255)
            .HasColumnName("content_type");
        entity.Property(e => e.Extension)
            .HasMaxLength(255)
            .HasColumnName("extension");
        entity.Property(e => e.FilePath)
            .HasMaxLength(255)
            .HasColumnName("file_path");
        entity.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name");
    }
}