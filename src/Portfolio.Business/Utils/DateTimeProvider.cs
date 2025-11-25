using Portfolio.Business.Extensions;

namespace Portfolio.Business.Utils;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateOnly Today => Now.ToDateOnly();
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now { get; } = DateTime.Now;
}