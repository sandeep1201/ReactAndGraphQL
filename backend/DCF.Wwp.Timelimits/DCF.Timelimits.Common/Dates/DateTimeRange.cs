using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using DCF.Common.Extensions;
using DCF.Common.Logging;

namespace DCF.Common.Dates
{

    [DebuggerDisplay("{Start} {DefaultSeparator} {End}")]
    public struct DateTimeRange : IEquatable<DateTimeRange>, IComparable<DateTimeRange>, IDateTimeRange
    {
        private ILog logger;
        public static readonly DateTimeRange Empty = new DateTimeRange(DateTime.MinValue, DateTime.MinValue);

        public const String DefaultSeparator = "/";

        public DateTimeRange(DateTime start) : this(start, DateTime.MaxValue) { }
        public DateTimeRange(DateTime start, DateTime end)
        {
            this.Start = start < end ? start : end;
            this.logger = LogProvider.GetLogger(typeof(DateTimeRange));
            this.UtcStart = this.Start != DateTime.MinValue ? this.Start.ToUniversalTime() : this.Start;
            this.End = end > start ? end : start;
            this.UtcEnd = this.End != DateTime.MaxValue ? this.End.ToUniversalTime() : this.End;
        }

        public DateTimeRange(DateTimeOffset start) : this(start, DateTimeOffset.MaxValue) { }
        public DateTimeRange(DateTimeOffset start, DateTimeOffset end)
        {
            this.Start = (start = start < end ? start : end).DateTime;
            this.logger = LogProvider.GetLogger(typeof(DateTimeRange));
            this.UtcStart = start.UtcDateTime;
            this.End = (end = end > start ? end : start).DateTime;
            this.UtcEnd = end.UtcDateTime;
        }

        public DateTime Start { get; }

        public DateTime End { get; }

        public DateTime UtcStart { get; }

        public DateTime UtcEnd { get; }

        public TimeSpan Duration => TimeSpan.FromTicks(this.End.Ticks - this.Start.Ticks);

        public DateTimeRange Add(DateTimeRange other)
        {
            return this.Overlaps(other) ? new DateTimeRange(this.Start.Min(other.Start), this.End.Max(other.End)) : DateTimeRange.Empty;
        }

        //public DateTimeRange Add(TimeSpan timeSpan)
        //{
        //    var offset = Start - UtcStart;
        //    return new DateTimeRange(new DateTimeOffset(Start.SafeAdd(timeSpan), offset), new DateTimeOffset(End.SafeAdd(timeSpan), offset));
        //}

        public Tuple<DateTimeRange?,DateTimeRange?> Subtract(DateTimeRange other)
        {
            var start = this.Start;
            var end = this.End;
            var oStart = other.Start;
            var oEnd = other.End;

            if (this.Intersection(other) == DateTimeRange.Empty)
            {
                return new Tuple<DateTimeRange?,DateTimeRange?>(this,null);
            }
            else if ((oStart <= start) && (start < end) && (end <= oEnd))
            {
                return new Tuple<DateTimeRange?, DateTimeRange?>(null,null);
            }
            else if ((oStart <= start) && (start < oEnd) && (oEnd < end))
            {
                return new Tuple<DateTimeRange?, DateTimeRange?>(new DateTimeRange(oEnd, end),null);
            }
            else if ((start < oStart) && (oStart < end) && (end <= oEnd))
            {
                return new Tuple<DateTimeRange?, DateTimeRange?>(new DateTimeRange(start, oStart), null);

            }
            else if ((start < oStart) && (oStart < oEnd) && (oEnd < end))
            {
                return new Tuple<DateTimeRange?, DateTimeRange?>(new DateTimeRange(start, oStart), new DateTimeRange(oEnd, end));
            }
            else if ((start < oStart) && (oStart < end) && (oEnd < end))
            {
                return new Tuple<DateTimeRange?, DateTimeRange?>(new DateTimeRange(start, oStart), new DateTimeRange(oStart, end));
            }

                return new Tuple<DateTimeRange?, DateTimeRange?>(null,null);
        }

        public DateTimeRange Subtract(TimeSpan timeSpan)
        {
            var offset = this.Start - this.UtcStart;
            return new DateTimeRange(new DateTimeOffset(this.Start.SafeSubtract(timeSpan), offset), new DateTimeOffset(this.End.SafeSubtract(timeSpan), offset));
        }

        public Boolean Intersects(DateTimeRange other)
        {
            return this.Intersection(other) != DateTimeRange.Empty;
        }

        public DateTimeRange Intersection(DateTimeRange other)
        {
            DateTime greatestStart = this.Start > other.Start ? this.Start : other.Start;
            DateTime smallestEnd = this.End < other.End ? this.End : other.End;

            if (greatestStart > smallestEnd)
                return DateTimeRange.Empty;

            return new DateTimeRange(greatestStart, smallestEnd);
        }

        public Boolean Contains(DateTimeRange dateTimeRange)
        {
            return this.Contains(dateTimeRange.Start) && this.Contains(dateTimeRange.End);
        }

        public Boolean Contains(DateTime time)
        {
            if (time.Kind == DateTimeKind.Utc)
                return time >= this.UtcStart && time <= this.UtcEnd;

            return time >= this.Start && time <= this.End;
        }


        public IEnumerable<DateTime> By(DateTimeUnits unit = DateTimeUnits.Milliseconds, Int32 step = 1)
        {
            if (this == DateTimeRange.Empty)
            {
                yield break;
            }

            var normalizedUnit = this.NormalizeUnit(unit);
            step = Math.Max(1, Math.Abs(step)); // make sure step is positive
            var current = this.Start.StartOf(normalizedUnit);
            var diff = Math.Abs(this.Start.Diff(this.End, unit)) / step;
            var count = 0;
            var isDateMax = false;
            do
            {
                yield return current;
                switch (unit)
                {
                    case DateTimeUnits.Years:
                        try
                        {
                            current = current.AddYears(step);
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            this.logger.WarnException("Iteration of DateTime by year hit DateTime Max, bailing at DateTime.Max", e);
                            isDateMax = true;
                        }

                        break;
                    case DateTimeUnits.Months:
                        try
                        {
                            current = current.AddMonths(step);

                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            this.logger.WarnException("Iteration of DateTime by month hit DateTime Max, bailing at DateTime.Max", e);
                            isDateMax = true;
                        }
                        break;
                    default:
                        var singleUnit = this.SingleUnitOfDuration(unit);
                        current = current.SafeAdd(new TimeSpan(singleUnit.Ticks * step));
                        break;
                }
                count++;
                if (current == DateTime.MaxValue)
                {
                    isDateMax = true;
                }
            } while (!isDateMax && count <= diff);

        }

        public IEnumerable<DateTime> ByReverse(DateTimeUnits unit = DateTimeUnits.Milliseconds, Int32 step = 1)
        {
            return this.By(unit, step).Reverse();
        }

        public Boolean Overlaps(DateTimeRange other)
        {
            var intersects = this.Intersects(other);

            return intersects;
        }

        public Boolean Adjacent(DateTimeRange other, DateTimeUnits precision = DateTimeUnits.Milliseconds)
        {
            // if they overlap for more than the precision amount, they can't be adjacent
            var intersection = this.Intersection(other);
            if (intersection != DateTimeRange.Empty && intersection.Duration >= this.SingleUnitOfDuration(precision))
            {
                return false;
            }

            var normalizedPrecision = this.NormalizeUnit(precision);
            var thisIsFirst = this.Start.IsBefore(other.Start);

            var first = thisIsFirst ? this : other;
            var second = thisIsFirst ? other : this;

            var gapDiff = first.End.StartOf(normalizedPrecision).DiffPrecise(second.Start.StartOf(normalizedPrecision), precision);

            return gapDiff <= 1;
            //var sameStartEnd = this.Start.IsSame(other.End, normalizedPrecision);
            //var sameEndStart = this.End.IsSame(other.Start, normalizedPrecision);

            //return (sameStartEnd && (other.Start.Millisecond <= this.Start.Millisecond)) || (sameEndStart && (other.End.Millisecond >= this.End.Millisecond));
        }


        public Boolean IsEqual(DateTimeRange other, DateTimeUnits unitOfTime = DateTimeUnits.Milliseconds)
        {
            return this.Start.IsSame(other.Start, this.NormalizeUnit(unitOfTime)) && this.End.IsSame(other.End, this.NormalizeUnit(unitOfTime));

        }

        public Boolean IsSame(DateTimeRange other, DateTimeUnits unitOfTime = DateTimeUnits.Milliseconds)
        {
            return this.IsEqual(other, unitOfTime);
        }

        #region Private methods

        private TimeSpan SingleUnitOfDuration(DateTimeUnits duration)
        {
            TimeSpan returnVal;
            switch (duration)
            {
                case DateTimeUnits.Years:
                    throw new NotSupportedException("Single year unit of unit not supported");
                case DateTimeUnits.Months:
                    throw new NotSupportedException("Single months unit of unit not supported");
                case DateTimeUnits.Weeks:
                    returnVal = TimeSpan.FromDays(7);
                    break;
                case DateTimeUnits.Days:
                    returnVal = TimeSpan.FromDays(1);
                    break;
                case DateTimeUnits.Hours:
                    returnVal = TimeSpan.FromHours(1);
                    break;
                case DateTimeUnits.Minutes:
                    returnVal = TimeSpan.FromMinutes(1);
                    break;
                case DateTimeUnits.Seconds:
                    returnVal = TimeSpan.FromSeconds(1);
                    break;
                case DateTimeUnits.Milliseconds:
                default:
                    returnVal = TimeSpan.FromMilliseconds(1);
                    break;
            }

            return returnVal;
        }

        private DateTimeUnit NormalizeUnit(DateTimeUnits unit)
        {
            switch (unit)
            {
                case DateTimeUnits.Years:
                    return DateTimeUnit.Year;
                case DateTimeUnits.Months:
                    return DateTimeUnit.Month;
                case DateTimeUnits.Weeks:
                    return DateTimeUnit.Week;
                case DateTimeUnits.Days:
                    return DateTimeUnit.Day;
                case DateTimeUnits.Hours:
                    return DateTimeUnit.Hour;
                case DateTimeUnits.Minutes:
                    return DateTimeUnit.Minute;
                case DateTimeUnits.Seconds:
                    return DateTimeUnit.Second;
                case DateTimeUnits.Milliseconds:
                    return DateTimeUnit.Millisecond;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }
        }

        #endregion

        #region Object overrides

        public override Int32 GetHashCode()
        {
            return (this.Start.Ticks + this.End.Ticks).GetHashCode();
        }

        public override String ToString()
        {
            return this.ToString(DateTimeRange.DefaultSeparator);
        }

        public String ToString(String separator)
        {
            return this.Start.ToString("s") + separator + this.End.ToString("s");
        }

        #endregion

        #region IEquatable implementation

        public static Boolean operator ==(DateTimeRange left, DateTimeRange right)
        {
            return (left.Start == right.Start) && (left.End == right.End);
        }

        public static Boolean operator !=(DateTimeRange left, DateTimeRange right)
        {
            return !(left == right);
        }

        

        public override Boolean Equals(Object obj)
        {
            if (obj == null)
                return false;

            var other = (DateTimeRange)obj;

            return (this.Start == other.Start) && (this.End == other.End);
        }

        public Boolean Equals(DateTimeRange other)
        {
            if ((Object)other == null)
                return false;

            return (this.Start == other.Start) && (this.End == other.End);
        }

        #endregion

        #region Comparison Operators
        // These are based on the length only
        public static Boolean operator >=(DateTimeRange left, DateTimeRange right)
        {
            return left.Duration >= right.Duration;
        }

        public static Boolean operator <=(DateTimeRange left, DateTimeRange right)
        {
            return left.Duration <= right.Duration;
        }

        public static Boolean operator <(DateTimeRange left, DateTimeRange right)
        {
            return left.Duration < right.Duration; ;
        }

        public static Boolean operator >(DateTimeRange left, DateTimeRange right)
        {
            return left.Duration > right.Duration;
        }

        #endregion

        #region IComparable implementation

        // Sorting by default will be by start, if equal they will then be sorted by enddate
        // TODO: 
        public Int32 CompareTo(DateTimeRange other)
        {
            if (this.Equals(other))
                return 0;

            var compareResult = this.Start.CompareTo(other.Start);
            if (compareResult == 0)
            {
                compareResult = other.End.CompareTo(this.End);
            }
            return compareResult;
        }

        #endregion
    }

}
