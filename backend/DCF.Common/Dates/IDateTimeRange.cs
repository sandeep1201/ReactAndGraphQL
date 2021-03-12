using System;
using System.Collections.Generic;

namespace DCF.Common.Dates
{
    public interface IDateTimeRange
    {
        DateTime End { get; }
        DateTime Start { get; }
        DateTime UtcEnd { get; }
        DateTime UtcStart { get; }

        DateTimeRange Add(DateTimeRange other);
        Boolean Adjacent(DateTimeRange other, DateTimeUnits precision = DateTimeUnits.Milliseconds);

        IEnumerable<System.DateTime> By(DateTimeUnits unit = DateTimeUnits.Milliseconds, Int32 step = 1);
        IEnumerable<System.DateTime> ByReverse(DateTimeUnits unit = DateTimeUnits.Milliseconds, Int32 step = 1);

        Int32 CompareTo(DateTimeRange other);
        Boolean Contains(DateTime time);
        Boolean Equals(DateTimeRange other);
        Boolean Equals(Object obj);
        Int32 GetHashCode();
        //DateTimeRange Intersection(DateTimeRange other);
        //Boolean Intersects(DateTimeRange other);
        Boolean Overlaps(DateTimeRange other);
        DateTimeRange Subtract(TimeSpan timeSpan);
        String ToString();
        String ToString(String separator);
    }
}