using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Dal.Entities.Common;

namespace Portfolio.Dal.Entities;

public class EmailVerificationToken : Entity
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public byte[] Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }
    public string? MessageId { get; set; }
}

public class EmailVerificationTokenEntityTypeConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        builder.ToTable("EmailVerificationToken");

        // Primary Key
        builder.HasKey(evt => evt.Id);

        builder.HasIndex(u => u.PublicId).IsUnique();
        builder.Property(u => u.PublicId)
            .IsRequired()
            .HasMaxLength(64);

        // Properties Configuration
        builder.Property(evt => evt.UserId)
            .IsRequired();

        builder.Property(evt => evt.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(evt => evt.ExpiresAt)
            .IsRequired();

        builder.Property(evt => evt.UsedAt)
            .IsRequired(false);

        builder.Property(evt => evt.MessageId)
            .HasMaxLength(500);

        builder.HasOne(evt => evt.User)
            .WithMany()
            .HasForeignKey(evt => evt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}