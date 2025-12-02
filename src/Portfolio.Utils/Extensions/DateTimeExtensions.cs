namespace Portfolio.Utils.Extensions;

public static class DateTimeExtensions
{
    public static DateOnly ToDateOnly(this DateTime datetime)
    {
        return new DateOnly(datetime.Year, datetime.Month, datetime.Day);
    }
}