using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Portfolio.Dal.Converters;

public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter()
        : base(dateOnly =>
                dateOnly.ToDateTime(TimeOnly.MinValue),
            dateTime => DateOnly.FromDateTime(dateTime))
    {
    }
}