using System;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCF.Timelimits.Common.Tests
{
    [TestClass]
    public class DateTimeQueryIsAfterTests
    {
        #region IsAfter
        [TestMethod]
        [Description("DateTime IsAfter without units")]
        public void datetime_is_after_without_unit()
        {
            var dateTime1 = new DateTime(2017, 3, 2, 3, 4, 5, 10);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2018, 3, 2, 3, 5, 5, 10)), false, "year is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2016, 3, 2, 3, 3, 5, 10)), true, "year is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 4, 2, 3, 4, 5, 10)), false, "month is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 2, 2, 3, 4, 5, 10)), true, "month is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 3, 3, 3, 4, 5, 10)), false, "day is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 3, 1, 3, 4, 5, 10)), true, "day is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 3, 2, 4, 4, 5, 10)), false, "hour is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 3, 2, 2, 4, 5, 10)), true, "hour is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 3, 2, 3, 5, 5, 10)), false, "minute is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 3, 2, 3, 3, 5, 10)), true, "minute is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 3, 2, 3, 4, 6, 10)), false, "second is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 3, 2, 3, 4, 4, 11)), true, "second is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 3, 2, 3, 4, 5, 10)), false, "millisecond match");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 3, 2, 3, 4, 5, 11)), false, "millisecond is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 3, 2, 3, 4, 5, 9)), true, "millisecond is earlier");
            Assert.AreEqual(dateTime1.IsAfter(dateTime1), false, "DateTimes are not after themselves");
            Assert.AreEqual(dateTime1, dateTime2, "IsAfter second should not change moment");
        }

        [TestMethod]
        [Description("DateTime IsAfter after year")]
        public void datetime_is_after_year()
        {
            var dateTime1 = new DateTime(2017, 1, 2, 3, 4, 5, 6);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 5, 6, 7, 8, 9, 10), DateTimeUnit.Year), false, "year match");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2019, 5, 6, 7, 8, 9, 10), DateTimeUnit.Year), false, "year is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2016, 5, 6, 7, 8, 9, 10), DateTimeUnit.Year), true, "year is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 1, 1, 1, 0, 0, 0).StartOf(DateTimeUnit.Year), DateTimeUnit.Year), false, "exact start of year");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2017, 11, 30, 23, 59, 59, 999).EndOf(DateTimeUnit.Year), DateTimeUnit.Year), false, "exact end of year");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2018, 1, 1, 1, 0, 0, 0).StartOf(DateTimeUnit.Year), DateTimeUnit.Year), false, "start of next year");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2016, 11, 30, 23, 59, 59, 999).EndOf(DateTimeUnit.Year), DateTimeUnit.Year), true, "end of previous year");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(1980, 11, 30, 23, 59, 59, 999).EndOf(DateTimeUnit.Year), DateTimeUnit.Year), true, "end of year far before");
            Assert.AreEqual(dateTime1.IsAfter(dateTime1, DateTimeUnit.Year), false, "same moments are not after the same year");
            Assert.AreEqual(dateTime1, dateTime2, "IsAfter year should not change moment");

        }

        [TestMethod]
        [Description("DateTime IsAfter after month")]
        public void datetime_is_after_month()
        {
            var dateTime1 = new DateTime(2011, 2, 3, 4, 5, 6, 7);

            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 2, 6, 7, 8, 9, 10), DateTimeUnit.Month), false, "month match");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2012, 2, 6, 7, 8, 9, 10), DateTimeUnit.Month), false, "year is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2010, 2, 6, 7, 8, 9, 10), DateTimeUnit.Month), true, "year is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 5, 6, 7, 8, 9, 10), DateTimeUnit.Month), false, "month is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 1, 6, 7, 8, 9, 10), DateTimeUnit.Month), true, "month is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 2, 1, 0, 0, 0, 0), DateTimeUnit.Month), false, "exact start of month");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 2, 28, 23, 59, 59, 999), DateTimeUnit.Month), false, "exact end of month");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 1, 0, 0, 0, 0), DateTimeUnit.Month), false, "start of next month");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 1, 27, 23, 59, 59, 999), DateTimeUnit.Month), true, "end of previous month");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2010, 12, 31, 23, 59, 59, 999), DateTimeUnit.Month), true, "later month but earlier year");
            Assert.AreEqual(dateTime1.IsAfter(dateTime1, DateTimeUnit.Month), false, "same moments are not after the same month");
            Assert.AreEqual(dateTime1, dateTime2, "IsAfter month should not change moment");
        }

        [TestMethod]
        [Description("DateTime IsAfter after day")]
        public void datetime_is_after_day()
        {
            var dateTime1 = new DateTime(2011, 3, 2, 3, 4, 5, 6);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 7, 8, 9, 10), DateTimeUnit.Day), false, "day match");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2012, 3, 2, 7, 8, 9, 10), DateTimeUnit.Day), false, "year is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2010, 3, 2, 7, 8, 9, 10), DateTimeUnit.Day), true, "year is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 4, 2, 7, 8, 9, 10), DateTimeUnit.Day), false, "month is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 2, 2, 7, 8, 9, 10), DateTimeUnit.Day), true, "month is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 3, 7, 8, 9, 10), DateTimeUnit.Day), false, "day is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 1, 7, 8, 9, 10), DateTimeUnit.Day), true, "day is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 0, 0, 0, 0), DateTimeUnit.Day), false, "exact start of day");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 23, 59, 59, 999), DateTimeUnit.Day), false, "exact end of day");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 3, 0, 0, 0, 0), DateTimeUnit.Day), false, "start of next day");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 1, 23, 59, 59, 999), DateTimeUnit.Day), true, "end of previous day");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2010, 3, 10, 0, 0, 0, 0), DateTimeUnit.Day), true, "later day but earlier year");
            Assert.AreEqual(dateTime1.IsAfter(dateTime1, DateTimeUnit.Day), false, "same moments are not after the same day");
            Assert.AreEqual(dateTime1, dateTime2, "IsAfter day should not change moment");
        }

        [TestMethod]
        [Description("DateTime IsAfter after year")]
        public void datetime_is_after_hour()
        {
            var dateTime1 = new DateTime(2011, 3, 2, 3, 4, 5, 6);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 8, 9, 10), DateTimeUnit.Hour), false, "hour match");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2012, 3, 2, 3, 8, 9, 10), DateTimeUnit.Hour), false, "year is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2010, 3, 2, 3, 8, 9, 10), DateTimeUnit.Hour), true, "year is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 4, 2, 3, 8, 9, 10), DateTimeUnit.Hour), false, "month is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 1, 2, 3, 8, 9, 10), DateTimeUnit.Hour), true, "month is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 3, 3, 8, 9, 10), DateTimeUnit.Hour), false, "day is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 1, 3, 8, 9, 10), DateTimeUnit.Hour), true, "day is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 4, 8, 9, 10), DateTimeUnit.Hour), false, "hour is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 8, 9, 10), DateTimeUnit.Hour), false, "hour is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 0, 0, 0), DateTimeUnit.Hour), false, "exact start of hour");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 59, 59, 999), DateTimeUnit.Hour), false, "exact end of hour");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 4, 0, 0, 0), DateTimeUnit.Hour), false, "start of next hour");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 2, 59, 59, 999), DateTimeUnit.Hour), true, "end of previous hour");
            Assert.AreEqual(dateTime1.IsAfter(dateTime1, DateTimeUnit.Hour), false, "same moments are not after the same hour");
            Assert.AreEqual(dateTime1, dateTime2, "IsAfter hour should not change moment");
        }

        [TestMethod]
        [Description("DateTime IsAfter after year")]
        public void datetime_is_after_minute()
        {
            var dateTime1 = new DateTime(2011, 3, 2, 3, 4, 5, 6);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 9, 10), DateTimeUnit.Minute), false, "minute match");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2012, 3, 2, 3, 4, 9, 10), DateTimeUnit.Minute), false, "year is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2010, 3, 2, 3, 4, 9, 10), DateTimeUnit.Minute), true, "year is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 4, 2, 3, 4, 9, 10), DateTimeUnit.Minute), false, "month is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 2, 2, 3, 4, 9, 10), DateTimeUnit.Minute), true, "month is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 3, 3, 4, 9, 10), DateTimeUnit.Minute), false, "day is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 1, 3, 4, 9, 10), DateTimeUnit.Minute), true, "day is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 4, 4, 9, 10), DateTimeUnit.Minute), false, "hour is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 2, 4, 9, 10), DateTimeUnit.Minute), true, "hour is earler");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 5, 9, 10), DateTimeUnit.Minute), false, "minute is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 3, 9, 10), DateTimeUnit.Minute), true, "minute is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 0, 0), DateTimeUnit.Minute), false, "exact start of minute");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 59, 999), DateTimeUnit.Minute), false, "exact end of minute");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 5, 0, 0), DateTimeUnit.Minute), false, "start of next minute");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 3, 59, 999), DateTimeUnit.Minute), true, "end of previous minute");
            Assert.AreEqual(dateTime1.IsAfter(dateTime1, DateTimeUnit.Minute), false, "same moments are not after the same minute");
            Assert.AreEqual(dateTime1, dateTime2, "IsAfter minute should not change moment");
        }

        [TestMethod]
        [Description("DateTime IsAfter is after second")]
        public void datetime_is_after_second()
        {
            var dateTime1 = new DateTime(2011, 3, 2, 3, 4, 5, 10);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 5, 10), DateTimeUnit.Second), false, "second match");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2012, 3, 2, 3, 4, 5, 10), DateTimeUnit.Second), false, "year is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2010, 3, 2, 3, 4, 5, 10), DateTimeUnit.Second), true, "year is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 4, 2, 3, 4, 5, 10), DateTimeUnit.Second), false, "month is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 2, 2, 3, 4, 5, 10), DateTimeUnit.Second), true, "month is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 3, 3, 4, 5, 10), DateTimeUnit.Second), false, "day is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 1, 1, 4, 5, 10), DateTimeUnit.Second), true, "day is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 4, 4, 5, 10), DateTimeUnit.Second), false, "hour is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 1, 4, 1, 5, 10), DateTimeUnit.Second), true, "hour is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 5, 5, 10), DateTimeUnit.Second), false, "minute is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 3, 5, 10), DateTimeUnit.Second), true, "minute is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 6, 10), DateTimeUnit.Second), false, "second is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 4, 5), DateTimeUnit.Second), true, "second is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 5, 0), DateTimeUnit.Second), false, "exact start of second");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 5, 999), DateTimeUnit.Second), false, "exact end of second");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 6, 0), DateTimeUnit.Second), false, "start of next second");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 4, 999), DateTimeUnit.Second), true, "end of previous second");
            Assert.AreEqual(dateTime1.IsAfter(dateTime1, DateTimeUnit.Second), false, "same moments are not after the same second");
            Assert.AreEqual(dateTime1, dateTime2, "IsAfter second should not change moment");
        }

        [TestMethod]
        [Description("DateTime IsAfter after millisecond")]
        public void datetime_is_after_millisecond()
        {
            var dateTime1 = new DateTime(2011, 3, 2, 3, 4, 5, 10);
            var dateTime2 = dateTime1;
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 5, 10), DateTimeUnit.Millisecond), false, "millisecond match");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2012, 3, 2, 3, 4, 5, 10), DateTimeUnit.Millisecond), false, "year is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2010, 3, 2, 3, 4, 5, 10), DateTimeUnit.Millisecond), true, "year is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 4, 2, 3, 4, 5, 10), DateTimeUnit.Millisecond), false, "month is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 2, 2, 3, 4, 5, 10), DateTimeUnit.Millisecond), true, "month is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 3, 3, 4, 5, 10), DateTimeUnit.Millisecond), false, "day is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 1, 1, 4, 5, 10), DateTimeUnit.Millisecond), true, "day is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 4, 4, 5, 10), DateTimeUnit.Millisecond), false, "hour is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 1, 4, 1, 5, 10), DateTimeUnit.Millisecond), true, "hour is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 5, 5, 10), DateTimeUnit.Millisecond), false, "minute is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 3, 5, 10), DateTimeUnit.Millisecond), true, "minute is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 6, 10), DateTimeUnit.Millisecond), false, "second is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 4, 5), DateTimeUnit.Millisecond), true, "second is earlier");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 6, 11), DateTimeUnit.Millisecond), false, "millisecond is later");
            Assert.AreEqual(dateTime1.IsAfter(new DateTime(2011, 3, 2, 3, 4, 4, 9), DateTimeUnit.Millisecond), true, "millisecond is earlier");
            Assert.AreEqual(dateTime1.IsAfter(dateTime1, DateTimeUnit.Millisecond), false, "same moments are not after the same millisecond");
            Assert.AreEqual(dateTime1, dateTime2, "IsAfter millisecond should not change moment");
        }

        #endregion
    }
}