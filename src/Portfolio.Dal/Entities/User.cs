using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Dal.Entities.Common;

namespace Portfolio.Dal.Entities;

public class User : Entity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    public bool IsEmailConfirmed { get; set; }
    public bool IsSuperAdmin { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    public ICollection<AccountUser> AccountUsers { get; set; } = new List<AccountUser>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
}

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        // Primary Key
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.PublicId).IsUnique();
        builder.Property(u => u.PublicId)
            .IsRequired()
            .HasMaxLength(64);

        // Properties Configuration
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.IsEmailConfirmed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(u => u.IsSuperAdmin)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.LastLoginAt)
            .IsRequired(false);

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_User_Email");

        builder.HasIndex(u => u.UserName)
            .IsUnique()
            .HasDatabaseName("IX_User_UserName");

        builder.HasIndex(u => u.CreatedAt)
            .HasDatabaseName("IX_User_CreatedAt");

        builder.HasMany(u => u.AccountUsers)
            .WithOne(au => au.User)
            .HasForeignKey(au => au.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.PasswordResetTokens)
            .WithOne(prt => prt.User)
            .HasForeignKey(prt => prt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}