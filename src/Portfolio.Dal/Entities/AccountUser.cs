using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Dal.Entities.Common;

namespace Portfolio.Dal.Entities;

public class AccountUser : Entity
{
    public int AccountId { get; set; }
    public Account Account { get; set; } = default!;

    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public AccountRole Role { get; set; }

    public DateTime JoinedAt { get; set; }
}

public class AccountUserEntityTypeConfiguration : IEntityTypeConfiguration<AccountUser>
{
    public void Configure(EntityTypeBuilder<AccountUser> builder)
    {
        builder.ToTable("AccountUser");

        builder.HasKey(au => au.Id);

        builder.HasIndex(u => u.PublicId).IsUnique();
        builder.Property(u => u.PublicId)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(au => au.AccountId)
            .IsRequired();

        builder.Property(au => au.UserId)
            .IsRequired();

        builder.Property(au => au.Role)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(au => au.JoinedAt)
            .IsRequired();

        builder.HasIndex(au => new { au.AccountId, au.UserId })
            .IsUnique()
            .HasDatabaseName("IX_AccountUser_AccountId_UserId");

        builder.HasIndex(au => au.UserId)
            .HasDatabaseName("IX_AccountUser_UserId");

        builder.HasIndex(au => au.AccountId)
            .HasDatabaseName("IX_AccountUser_AccountId");

        builder.HasIndex(au => au.Role)
            .HasDatabaseName("IX_AccountUser_Role");

        builder.HasOne(au => au.Account)
            .WithMany(a => a.Members)
            .HasForeignKey(au => au.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(au => au.User)
            .WithMany(u => u.AccountUsers)
            .HasForeignKey(au => au.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public enum AccountRole
{
    /// <summary>
    /// Owner of the account with full permissions.
    /// </summary>
    Owner = 0,

    /// <summary>
    /// Administrator with elevated permissions.
    /// </summary>
    Admin = 1,

    /// <summary>
    /// Standard member with basic permissions.
    /// </summary>
    Member = 2
}