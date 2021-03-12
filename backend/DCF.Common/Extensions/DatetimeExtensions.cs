using System;
using System.Globalization;
using DCF.Common.Dates;

namespace DCF.Common.Extensions
{
    public static class DatetimeExtensions
    {
        public static DateTime SafeAdd(this DateTime date, TimeSpan value)
        {
            if (date.Ticks + value.Ticks < DateTime.MinValue.Ticks)
                return DateTime.MinValue;

            if (date.Ticks + value.Ticks > DateTime.MaxValue.Ticks)
                return DateTime.MaxValue;

            return date.Add(value);
        }

        public static DateTime SafeSubtract(this DateTime date, TimeSpan value)
        {
            if (date.Ticks - value.Ticks < DateTime.MinValue.Ticks)
                return DateTime.MinValue;

            if (date.Ticks - value.Ticks > DateTime.MaxValue.Ticks)
                return DateTime.MaxValue;

            return date.Subtract(value);
        }

        public static DateTime Min(this DateTime x, DateTime y)
        {
            return (x.ToUniversalTime() < y.ToUniversalTime()) ? x : y;
        }

        public static DateTime Max(this DateTime x, DateTime y)
        {
            return (x.ToUniversalTime() > y.ToUniversalTime()) ? x : y;
        }

        #region Query

        /// <summary>
        ///     Check if this DateTime is before <paramref name="compareDateTime"></paramref>
        /// </summary>
        /// <param name="dateTime">this DateTime</param>
        /// <param name="compareDateTime">Compare Date</param>
        /// <param name="unit"></param>
        /// <returns>boolean</returns>
        /// <example>10/20/2010 isBefore 12/31/2010, DateTimeUnit.Year = false</example>
        /// <example>10/20/2010 isBefore 01/01/2011, DateTimeUnit.Year = true</example>
        public static Boolean IsBefore(this DateTime dateTime, DateTime compareDateTime, DateTimeUnit unit = DateTimeUnit.Millisecond)
        {
            return dateTime.EndOf(unit) < compareDateTime.EndOf(unit);
        }

        public static Boolean IsSameOrBefore(this DateTime dateTime, DateTime compareDateTime, DateTimeUnit unit = DateTimeUnit.Millisecond)
        {
            return dateTime.IsSame(compareDateTime, unit) || dateTime.IsBefore(compareDateTime, unit);
        }

        /// <summary>
        ///     Check if this DateTime is after <paramref name="compareDateTime"></paramref>, optionally at
        ///     <paramref name="unit"></paramref>
        /// </summary>
        /// <param name="dateTime">this DateTime</param>
        /// <param name="compareDateTime">date to compare</param>
        /// <param name="unit"><see cref="DateTimeUnit"/></param>
        /// <returns>boolean</returns>
        public static Boolean IsAfter(this DateTime dateTime, DateTime compareDateTime, DateTimeUnit unit = DateTimeUnit.Millisecond)
        {
            return compareDateTime.StartOf(unit) < dateTime.StartOf(unit);
        }

        /// <summary>
        ///     Check if this DateTime is the same as <paramref name="compareDateTime"></paramref>, optionally at
        ///     <paramref name="unit"></paramref>
        /// </summary>
        /// <param name="dateTime">this DateTime</param>
        /// <param name="compareDateTime">date to compare</param>
        /// <param name="unit"><see cref="DateTimeUnit"/></param>
        /// <returns>boolean</returns>
        public static Boolean IsSame(this DateTime dateTime, DateTime compareDateTime, DateTimeUnit unit = DateTimeUnit.Millisecond)
        {
            return dateTime.StartOf(unit) <= compareDateTime.StartOf(unit) && compareDateTime.EndOf(unit) <= dateTime.EndOf(unit);
        }

        public static Boolean IsSameOrAfter(this DateTime dateTime, DateTime compareDateTime,
                                            DateTimeUnit         unit = DateTimeUnit.Millisecond)
        {
            return dateTime.IsSame(compareDateTime, unit) || dateTime.IsAfter(compareDateTime, unit);
        }

        /// <summary>
        ///     Check if this DateTime is between <paramref name="fromDate"></paramref> and <paramref name="toDate"></paramref>,
        ///     optionally at <paramref name="unit"></paramref>
        /// </summary>
        /// <param name="dateTime">this DateTime</param>
        /// <param name="fromDate">Start Date</param>
        /// <param name="toDate">End Date</param>
        /// <param name="unit"><see cref="DateTimeUnit"/></param>
        /// <returns>boolean</returns>
        public static Boolean IsBetween(this DateTime dateTime, DateTime fromDate, DateTime toDate,
                                        DateTimeUnit         unit = DateTimeUnit.Millisecond)
        {
            return dateTime.IsAfter(fromDate, unit) && dateTime.IsBefore(toDate, unit);
        }

        #endregion

        #region Manipulate

        /// <summary>
        ///     Get the start of this <paramref name="dateTime" /> at <paramref name="unit" />. E.g.
        ///     DateTime.Now.StartOf(DateTimeUnit.Year)
        ///     return a new <see cref="DateTime" /> at the start of the current year.
        /// </summary>
        /// <param name="dateTime">this DateTime</param>
        /// <param name="unit"><see cref="DateTimeUnit"/></param>
        /// <returns>DateTime at the start of give <paramref name="unit"/></returns>
        public static DateTime StartOf(this DateTime dateTime, DateTimeUnit unit)
        {
            switch (unit)
            {
                case DateTimeUnit.Year:
                    return new DateTime(dateTime.Year, 1, 1);
                case DateTimeUnit.Month:
                    return new DateTime(dateTime.Year, dateTime.Month, 1);
                case DateTimeUnit.Week:
                    dateTime = dateTime.StartOf(DateTimeUnit.Day);
                    var ci      = CultureInfo.CurrentCulture;
                    var first   = (Int32) ci.DateTimeFormat.FirstDayOfWeek;
                    var current = (Int32) dateTime.DayOfWeek;
                    return first <= current
                               ? dateTime.AddDays(-1 * (current - first))
                               : dateTime.AddDays(first - current - 7);
                case DateTimeUnit.Day:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
                case DateTimeUnit.Hour:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, 0);
                case DateTimeUnit.Minute:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
                case DateTimeUnit.Second:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, 0);
                default:
                    return dateTime;
            }
        }

        /// <summary>
        ///     Get the end of <paramref name="unit" /> from DateTime.UtcNow. E.g. EndOf(DateTimeUnit.Year)
        ///     return a new <see cref="DateTime" /> at the end of the current year.
        /// </summary>
        /// <param name="dateTime">this DateTime</param>
        /// <param name="unit"><see cref="DateTimeUnit"/></param>
        /// <returns>DateTime at the end of give <paramref name="unit"/></returns>
        public static DateTime EndOf(this DateTime dateTime, DateTimeUnit unit)
        {
            switch (unit)
            {
                case DateTimeUnit.Year:
                    //return StartOf(dateTime, DateTimeUnit.Year).AddYears(1).AddSeconds(-1);
                    return new DateTime(dateTime.Year, 12, 1).EndOf(DateTimeUnit.Month);
                case DateTimeUnit.Month:
                    //return StartOf(dateTime, DateTimeUnit.Month).AddMonths(1).AddSeconds(-1);
                    return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month)).EndOf(DateTimeUnit.Day);

                case DateTimeUnit.Week:
                    return StartOf(dateTime, DateTimeUnit.Week).AddDays(7).AddSeconds(-1);
                //return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year,dateTime.Month));

                case DateTimeUnit.Day:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 0);
                case DateTimeUnit.Hour:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 59, 59, 0);
                case DateTimeUnit.Minute:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 59, 0);
                case DateTimeUnit.Second:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute,
                                               dateTime.Second, 999);
                case DateTimeUnit.Millisecond:
                    return dateTime;
                default:
                    throw new ArgumentException("No valid unit was provided", nameof(unit));
            }
        }

        #endregion

        public static Double DiffPrecise(this DateTime x, DateTime y, DateTimeUnits units = DateTimeUnits.Milliseconds)
        {
            Double returnVal = 0;
            if (units == DateTimeUnits.Years || units == DateTimeUnits.Months)
            {
                returnVal = MonthDiff(x, y);
                if (units == DateTimeUnits.Years)
                {
                    returnVal = returnVal / 12;
                }
            }
            else
            {
                var timespan = (x - y);
                switch (units)
                {
                    case DateTimeUnits.Weeks:
                        //returnVal = (timespan.TotalMilliseconds / (1000 * 60 * 60 * 24 * 7));
                        returnVal = (timespan.TotalMilliseconds / TimeSpan.FromDays(7).TotalMilliseconds);
                        break;
                    case DateTimeUnits.Days:
                        returnVal = (timespan.TotalMilliseconds / TimeSpan.FromDays(1).TotalMilliseconds);
                        break;
                    case DateTimeUnits.Hours:
                        returnVal = (timespan.TotalMilliseconds / TimeSpan.FromHours(1).TotalMilliseconds);
                        break;
                    case DateTimeUnits.Minutes:
                        returnVal = (timespan.TotalMilliseconds / TimeSpan.FromMinutes(1).TotalMilliseconds);
                        break;
                    case DateTimeUnits.Seconds:
                        returnVal = (timespan.TotalMilliseconds / TimeSpan.FromSeconds(1).TotalMilliseconds);
                        break;
                    case DateTimeUnits.Milliseconds:
                        returnVal = timespan.TotalMilliseconds;
                        break;
                }
            }

            return Math.Abs(returnVal);
        }

        public static Double Diff(this DateTime x, DateTime y, DateTimeUnits units = DateTimeUnits.Milliseconds)
        {
            return Math.Abs(Math.Floor(DiffPrecise(x, y, units)));
        }

        private static Int32 MonthDiff(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }
    }

    public static class ToDateTimeMonthYearExtensions
    {
        public static DateTime? ToDateTimeMonthYear(this string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return null;

            var mm   = input.Split('/')[0];
            var yyyy = input.Split('/')[1];

            int mmp   = Int32.Parse(mm);
            int yyyyp = Int32.Parse(yyyy);

            try
            {
                return new DateTime(yyyyp, mmp, 1);
            }
            catch
            {
                return null;
            }
        }

        public static DateTime? ToDateTimeMonth(this string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return null;

            var mm  = input.Split('/')[0];
            int mmp = Int32.Parse(mm);


            try
            {
                return new DateTime(mmp);
            }
            catch
            {
                return null;
            }
        }

        public static DateTime? ToDateTimeMonthDayYear(this string input1)
        {
            // TODO: This should use the DateTime.Parse method instead of this.

            if (String.IsNullOrWhiteSpace(input1))
                return null;

            var mm   = input1.Split('/')[0];
            var dd   = input1.Split('/')[1];
            var yyyy = input1.Split('/')[2];

            int mmp   = Int32.Parse(mm);
            int ddp   = Int32.Parse(dd);
            int yyyyp = Int32.Parse(yyyy);

            try
            {
                return new DateTime(yyyyp, mmp, ddp);
            }
            catch
            {
                return null;
            }
        }

        public static DateTime? ToValidDateTimeMonthDayYear(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            if (DateTime.TryParse(value, out var dateTime)) return dateTime;

            return null;
        }

        public static string ToStringMonthDayYear(this DateTime? input)
        {
            return input?.ToString("MM/dd/yyyy");
        }

        public static string ToStringMonthYear(this DateTime? input)
        {
            return input?.ToString("MM/yyyy");
        }

        /// <summary>
        ///  Returns DateTime as yyyyMM. Example 01/02/2012 as 201202.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToStringMonthYearComposite(this DateTime? input)
        {
            return !input.HasValue ? null : input.Value.ToStringMonthYearComposite();
        }

        /// <summary>
        ///  Returns DateTime as yyyyMM. Example 01/02/2012 as 201202.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToStringMonthYearComposite(this DateTime input)
        {
            return input == default(DateTime) ? null : input.ToString("yyyyMM", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns DateTime as YYYY-MM-DD. Example 01/02/2012 as 2012-01-02.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToStringDashedYearMonthDay(this DateTime? input)
        {
            return input?.ToString("yyyy-MM-dd");
        }

        public static int? ToIntYear(this DateTime? input)
        {
            return input?.Year;
        }
    }
}
