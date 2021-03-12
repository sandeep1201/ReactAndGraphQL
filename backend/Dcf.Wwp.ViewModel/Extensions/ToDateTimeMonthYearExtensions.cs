using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace Dcf.Wwp.Api.Library.Extensions
{
    public static class ToDateTimeMonthYearExtensions
    {
        public static DateTime? ToDateTimeMonthYear(this string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 7)
                return null;

            var mm   = input.Split('/')[0];
            var yyyy = input.Split('/')[1];

            var mmp   = int.Parse(mm);
            var yyyyp = int.Parse(yyyy);

            // Sanity check on the year.  It must be 4 digits.
            if (yyyyp < 1000)
                return null;

            try
            {
                return new DateTime(yyyyp, mmp, 1);
            }
            catch
            {
                return null;
            }
        }

        public static DateTime? ToDateTimeMonthDayYear(this string value) // scott v.
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            DateTime.TryParse(value, out var dateTime);

            return (dateTime);
        }

        public static DateTime? ToMonthDateYearTime(this string value) // Silam T.
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            DateTime.TryParseExact(value, "MMddyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime);

            return (dateTime);
        }

        public static DateTime ToDateMonthDayYear(this string value) // Silam T.
        {
            var dateTime = DateTime.Parse(value);

            return (dateTime);
        }

        public static string ToStringMonthDayYear(this DateTime? input)
        {
            return input?.ToString("MM/dd/yyyy");
        }

        public static string ToStringMonthDayYear(this DateTime input)
        {
            return input.ToString("MM/dd/yyyy");
        }

        public static string ToStringMonthYear(this DateTime? input)
        {
            return input?.ToString("MM/yyyy");
        }

        public static IEnumerable<DateTime> GetDates(this DateTime? startDate, DateTime? endDate)
        {
            var dates = new List<DateTime>();
            if (startDate == null || endDate == null) return dates;
            var sd = (DateTime) startDate;
            var ed = (DateTime) endDate;

            dates = Enumerable.Range(0, 1 + ed.Subtract(sd).Days)
                              .Select(offset => sd.AddDays(offset))
                              .ToList();

            return dates;
        }

        public static IEnumerable<DateTime> GetDates(this DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                              .Select(offset => startDate.AddDays(offset))
                              .ToList();
        }

        //WILL display a leading zero in times such as 01:00 am
        public static string GetHourMinuteAndAMPM(this TimeSpan? time)
        {
            string timeString = null;

            if (time != null)
                timeString = DateTime.Today.Add((TimeSpan) time).ToString("hh:mm tt");

            return timeString;
        }

        //Will NOT display a leading zero in times such as 1:00 am  with lowercase am or pm
        public static string GetShortHourMinuteAndAMPM(this TimeSpan? time)
        {
            string timeString = null;

            if (time != null)
                timeString = DateTime.Today.Add((TimeSpan) time).ToString("h:mm tt").ToLower();

            return timeString;
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Sunday, string week = "current")
        {
            var date = dt;
            date = week == "current" ? date : week == "next" ? date.AddDays(7) : week == "previous" ? date.AddDays(-7) : week == "next-two" ? date.AddDays(14) : date;
            var diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-1                          * diff).Date;
        }

        public static DateTime LastDayOfWeek(DateTime date, DayOfWeek startOfWeek = DayOfWeek.Sunday, string week = "current")
        {
            return StartOfWeek(date, startOfWeek, week).AddDays(6);
        }

        public static DateTime FirstDayOfMonth(this DateTime dt)
        {
            var firstDt = new DateTime(dt.Year, dt.Month, 1);

            return firstDt;
        }

        public static DateTime SpecificDayOfMonth(this DateTime dt, int day, string period = "current")
        {
            var firstDt    = dt.FirstDayOfMonth();
            var specificDt = period == "current" ? firstDt.AddDays(day - 1) : period == "previous" ? firstDt.AddMonths(-1).AddDays(day - 1) : firstDt.AddMonths(1).AddDays(day - 1);

            return specificDt;
        }

        public static string ToMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }

        public static int MonthDiff(this DateTime lValue, DateTime rValue)
        {
            var noOfMonths = Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));

            if (rValue.Day < lValue.Day) noOfMonths--;

            return noOfMonths;
        }

        public static int DateDiff(this DateTime lValue, DateTime rValue)
        {
            return (rValue - lValue).Days;
        }

        public static int DateDiffIncludeCurrent(this DateTime lValue, DateTime rValue)
        {
            return (rValue.AddDays(1) - lValue).Days;
        }

        public static bool WithinAYear(this DateTime lValue, DateTime rValue)
        {
            return lValue < rValue ? rValue < lValue.AddYears(1) : lValue < rValue.AddYears(1);
        }

        public static string GetParticipationPeriodName(this DateTime beginDate, DateTime endDate)
        {
            return $"{beginDate:MMMM} 16 - {endDate:MMMM} 15";
        }
    }
}
