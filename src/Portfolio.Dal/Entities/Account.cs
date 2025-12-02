using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Dal.Entities.Common;

namespace Portfolio.Dal.Entities;

public class Account : Entity
{
    public string Name { get; set; } = default!;
    public string Slug { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public int OwnerId { get; set; }
    public User Owner { get; set; } = default!;

    public ICollection<AccountUser> Members { get; set; } = new List<AccountUser>();
}

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Account");

        builder.HasKey(a => a.Id);

        builder.HasIndex(u => u.PublicId).IsUnique();
        builder.Property(u => u.PublicId)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.OwnerId)
            .IsRequired();

        builder.HasIndex(a => a.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Account_Slug");

        builder.HasIndex(a => a.OwnerId)
            .HasDatabaseName("IX_Account_OwnerId");

        builder.HasIndex(a => a.CreatedAt)
            .HasDatabaseName("IX_Account_CreatedAt");

        builder.HasOne(a => a.Owner)
            .WithMany()
            .HasForeignKey(a => a.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Members)
            .WithOne(au => au.Account)
            .HasForeignKey(au => au.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
