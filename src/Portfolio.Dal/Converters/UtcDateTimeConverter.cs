using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Portfolio.Dal.Converters;

public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter()
        : base(
            // To provider (database) - convert to UTC if not already
            v => v.Kind == DateTimeKind.Utc
                ? v
                : v.Kind == DateTimeKind.Local
                    ? v.ToUniversalTime()
                    : DateTime.SpecifyKind(v, DateTimeKind.Utc),
            // From provider (database) - always UTC from Postgres
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
    {
    }
}