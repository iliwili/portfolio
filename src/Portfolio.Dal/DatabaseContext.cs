using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Portfolio.Dal.Converters;
using Portfolio.Dal.Entities;
using Portfolio.Dal.Entities.Common;
using Portfolio.Dal.Utils;
using Portfolio.Utils;


namespace Portfolio.Dal;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountUser> AccountUsers => Set<AccountUser>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
    public DbSet<EmailVerificationToken> EmailVerificationTokens => Set<EmailVerificationToken>();

#if DEBUG || DEBUGTEST
    public const string SystemUser = "SYSTEMDEV";
#else
    public const string SystemUser = "SYSTEM";
#endif

    private readonly DbContextOptions<DatabaseContext> _options;
    private readonly ICurrentUser _currentUser;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DatabaseContext(
        DbContextOptions<DatabaseContext> options,
        ICurrentUser currentUser,
        IDateTimeProvider dateTimeProvider)
        : base(options)
    {
        _options = options;
        _currentUser = currentUser;
        _dateTimeProvider = dateTimeProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IPortfolioDayLayer).Assembly);

        // Apply the configuration for all entities inheriting from Entity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(Entity).IsAssignableFrom(entityType.ClrType) &&
                entityType.ClrType.BaseType == typeof(Entity) &&
                entityType.ClrType != typeof(Entity))
            {
                var entityConfigType = typeof(EntityEntityTypeConfiguration<>).MakeGenericType(entityType.ClrType);
                var entityConfigInstance = Activator.CreateInstance(entityConfigType);
                modelBuilder.ApplyConfiguration((dynamic)entityConfigInstance);
            }

            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var entityConfigType = typeof(EntitySoftDeleteEntityTypeConfiguration<>).MakeGenericType(entityType.ClrType);
                var entityConfigInstance = Activator.CreateInstance(entityConfigType);
                modelBuilder.ApplyConfiguration((dynamic)entityConfigInstance);
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");

        configurationBuilder.Properties<TimeOnly>()
            .HaveConversion<TimeOnlyConverter>()
            .HaveColumnType("time");

        base.ConfigureConventions(configurationBuilder);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries().ToList();

        EnrichWithAuditInfo(entries);
        UpdateLockIds(entries);

        try
        {
            var affectedRows = await base.SaveChangesAsync(cancellationToken);
            return affectedRows;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new DbUpdateConcurrencyException("A concurrency conflict occurred.", ex);
        }
    }

    private void EnrichWithAuditInfo(List<EntityEntry> entries)
    {
        // Added entries
        foreach (var entry in entries.Where(x => x.State == EntityState.Added))
        {
            switch (entry.Entity)
            {
                case Entity entity:
                {
                    if (string.IsNullOrWhiteSpace(entity.PublicId))
                    {
                        var prefix = PublicIdGenerator.GetPrefixFor(entry.Entity.GetType());
                        entity.PublicId = PublicIdGenerator.GeneratePublicId(prefix);
                    }

                    entity.CreatedOn = _dateTimeProvider.Now;
                    entity.CreatedBy = _currentUser.UserName ?? SystemUser;
                    break;
                }
            }
        }

        // Modified entries
        foreach (var entry in entries.Where(x => x.State == EntityState.Modified))
        {
            switch (entry.Entity)
            {
                case Entity entity:
                {
                    entry.Property(nameof(Entity.CreatedOn)).IsModified = false;
                    entry.Property(nameof(Entity.CreatedBy)).IsModified = false;
                    entity.ModifiedBy = _currentUser.UserName ?? SystemUser;
                    entity.ModifiedOn = _dateTimeProvider.Now;
                    break;
                }
            }
        }
    }
    private static void UpdateLockIds(List<EntityEntry> entries)
    {
        // Changed entries
        foreach (var entry in entries.Where(x => x.State == EntityState.Added || x.State == EntityState.Modified))
        {
            if (entry.Entity is IConcurrencyTokenProperty lockedEntry)
            {
                // https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations
                // https://github.com/dotnet/efcore/issues/18505
                // make concurrency check work over api:
                entry.Property(nameof(IConcurrencyTokenProperty.LockId)).OriginalValue = lockedEntry.LockId;

                // Refresh token
                lockedEntry.LockId = Guid.NewGuid();
            }
        }
    }
}