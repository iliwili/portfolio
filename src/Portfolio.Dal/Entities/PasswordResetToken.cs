using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Dal.Entities.Common;

namespace Portfolio.Dal.Entities;

public class PasswordResetToken : Entity
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public byte[] Token { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }
    public string? MessageId { get; set; }
}

public class PasswordResetTokenEntityTypeConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("PasswordResetToken");

        // Primary Key
        builder.HasKey(prt => prt.Id);

        builder.HasIndex(u => u.PublicId).IsUnique();
        builder.Property(u => u.PublicId)
            .IsRequired()
            .HasMaxLength(64);

        // Properties Configuration
        builder.Property(prt => prt.UserId)
            .IsRequired();

        builder.Property(prt => prt.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(prt => prt.CreatedAt)
            .IsRequired();

        builder.Property(prt => prt.ExpiresAt)
            .IsRequired();

        builder.Property(prt => prt.UsedAt)
            .IsRequired(false);

        builder.Property(prt => prt.MessageId)
            .IsRequired(false)
            .HasMaxLength(255);

        // Indexes
        builder.HasIndex(prt => prt.Token)
            .IsUnique()
            .HasDatabaseName("IX_PasswordResetToken_Token");

        builder.HasIndex(prt => prt.UserId)
            .HasDatabaseName("IX_PasswordResetToken_UserId");

        builder.HasIndex(prt => prt.ExpiresAt)
            .HasDatabaseName("IX_PasswordResetToken_ExpiresAt");

        builder.HasIndex(prt => new { prt.UserId, prt.ExpiresAt })
            .HasDatabaseName("IX_PasswordResetToken_UserId_ExpiresAt");

        // Relationships
        builder.HasOne(prt => prt.User)
            .WithMany(u => u.PasswordResetTokens)
            .HasForeignKey(prt => prt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}