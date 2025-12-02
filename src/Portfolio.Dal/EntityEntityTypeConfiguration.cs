﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Dal.Entities.Common;
using Portfolio.Dal.Utils;

namespace Portfolio.Dal;

public class EntityEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : Entity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasDefaultValue("SYSTEM")
            .HasMaxLength(64);

        builder.Property(e => e.CreatedOn)
            .HasDefaultValue(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc))
            .IsRequired();

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(64);

        builder.Property(e => e.ModifiedOn)
            .IsRequired(false);

        builder.Property(e => e.LockId)
            .IsConcurrencyToken()
            .IsRequired();
    }
}

public class EntitySoftDeleteEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, ISoftDelete
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasQueryFilter(r => !r.IsDeleted);
        builder.HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
    }
}