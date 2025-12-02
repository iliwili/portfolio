using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Dal.Entities.Common;

namespace Portfolio.Dal.Entities;

public class RefreshToken : Entity
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public string Token { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }
    public string? ReplacedByToken { get; set; }
}

public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshToken");

        builder.HasKey(rt => rt.Id);

        builder.HasIndex(u => u.PublicId).IsUnique();
        builder.Property(u => u.PublicId)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(rt => rt.CreatedAt)
            .IsRequired();

        builder.Property(rt => rt.ExpiresAt)
            .IsRequired();

        builder.Property(rt => rt.RevokedAt)
            .IsRequired(false);

        builder.Property(rt => rt.ReplacedByToken)
            .HasMaxLength(500);

        builder.HasIndex(rt => rt.Token)
            .IsUnique()
            .HasDatabaseName("IX_RefreshToken_Token");

        builder.HasIndex(rt => rt.ExpiresAt)
            .HasDatabaseName("IX_RefreshToken_ExpiresAt");

        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}