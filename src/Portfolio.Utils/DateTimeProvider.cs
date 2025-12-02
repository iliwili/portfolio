﻿using Portfolio.Utils.Extensions;

namespace Portfolio.Utils;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateOnly Today => Now.ToDateOnly();
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}