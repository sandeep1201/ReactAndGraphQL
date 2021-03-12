using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DCF.Timelimits.Common.Tests.Dates
{
    [TestClass]
    public class DateTimeRangeTests
    {
        private DateTime d1 = new DateTime(2011, 2, 5, 0, 0, 0, DateTimeKind.Utc);
        private DateTime d2 = new DateTime(2011, 5, 5, 0, 0, 0, DateTimeKind.Utc);
        private DateTime d3 = new DateTime(2011, 4, 9, 0, 0, 0, DateTimeKind.Utc);
        private DateTime d4 = new DateTime(1988, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private DateTime m1 = DateTime.ParseExact("06-05-1996", "MM-dd-yyyy", CultureInfo.InvariantCulture);
        private DateTime m2 = DateTime.ParseExact("11-05-1996", "MM-dd-yyyy", CultureInfo.InvariantCulture);
        private DateTime m3 = DateTime.ParseExact("08-12-1996", "MM-dd-yyyy", CultureInfo.InvariantCulture);
        private DateTime m4 = DateTime.ParseExact("01-01-2012", "MM-dd-yyyy", CultureInfo.InvariantCulture);
        private string sStart = "1996-08-12T00:00:00.000Z";
        private string sEnd = "2012-01-01T00:00:00.000Z";

        private DateTime d5 = new DateTime(2011, 2, 2, 0, 0, 0, DateTimeKind.Utc);
        private DateTime d6 = new DateTime(2011, 4, 4, 0, 0, 0, DateTimeKind.Utc);
        private DateTime d7 = new DateTime(2011, 6, 6, 0, 0, 0, DateTimeKind.Utc);
        private DateTime d8 = new DateTime(2011, 8, 8, 0, 0, 0, DateTimeKind.Utc);

        [TestMethod]
        public void date_range_constructor_with_datetimes_test()
        {
            // Arrange
            var d1 = new DateTimeRange(new DateTime(2017, 1, 1), new DateTime(2017, 2, 1));
            var startMinDateTime = new DateTimeRange(DateTime.MinValue, DateTime.Now);
            var endMaxDateTime = new DateTimeRange(new DateTime(2017, 1, 1));
            // Act
            // Assert
            Assert.IsInstanceOfType(d1.Start, typeof(DateTime), "Should create DateTimeRange with a start DateTime");
            Assert.IsInstanceOfType(d1.End, typeof(DateTime), "Should create DateTimeRange with a end DateTime");
            Assert.IsInstanceOfType(d1.UtcStart, typeof(DateTime), "Should create DateTimeRange with a UtcStart DateTime");
            Assert.IsInstanceOfType(d1.UtcEnd, typeof(DateTime), "Should create DateTimeRange with a UtcEnd DateTime");

            Assert.IsTrue(d1.UtcStart.Kind == DateTimeKind.Utc, "Should create DateTimeRange with a UtcStart DateTime with Kind UTC");
            Assert.IsTrue(d1.UtcEnd.Kind == DateTimeKind.Utc, "Should create DateTimeRange with a UtcEnd DateTime with Kind UTC");

            Assert.IsTrue(startMinDateTime.Start.Equals(DateTime.MinValue), "Should create DateTimeRange with minumim start date ");
            Assert.IsTrue(endMaxDateTime.End.Equals(DateTime.MaxValue), "Should create DateTimeRange with maximum end date ");
        }

        [TestMethod]

        public void date_range_constructor_with_datetimeoffsets_test()
        {
            // Arrange
            var d1 = new DateTimeRange(new DateTimeOffset(2017, 1, 1, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2017, 1, 1, 0, 0, 0, 0, TimeSpan.Zero));
            var startMinDateTime = new DateTimeRange(DateTime.MinValue, DateTimeOffset.Now);
            var endMaxDateTime = new DateTimeRange(new DateTimeOffset(2017, 1, 1, 0, 0, 0, 0, TimeSpan.Zero));
            // Act
            // Assert
            Assert.IsInstanceOfType(d1.Start, typeof(DateTime), "Should create DateTimeRange with a start DateTime");
            Assert.IsInstanceOfType(d1.End, typeof(DateTime), "Should create DateTimeRange with a end DateTime");
            Assert.IsInstanceOfType(d1.UtcStart, typeof(DateTime), "Should create DateTimeRange with a UtcStart DateTime");
            Assert.IsInstanceOfType(d1.UtcEnd, typeof(DateTime), "Should create DateTimeRange with a UtcEnd DateTime");

            Assert.IsTrue(d1.UtcStart.Kind == DateTimeKind.Utc, "Should create DateTimeRange with a UtcStart DateTime with Kind UTC");
            Assert.IsTrue(d1.UtcEnd.Kind == DateTimeKind.Utc, "Should create DateTimeRange with a UtcEnd DateTime with Kind UTC");

            Assert.IsTrue(startMinDateTime.Start.Equals(DateTime.MinValue), "Should create DateTimeRange with minumim start date ");
            Assert.IsTrue(endMaxDateTime.End.Equals(DateTime.MaxValue), "Should create DateTimeRange with maximum end date ");
        }


        #region adjacent Tests

        [TestMethod]
        [Description("Adjacent should correctly indicate when ranges aren\"t adjacent")]
        public void Adjacent_should_correctly_indicate_when_ranges_arent_adjacent()
        {
            var a = new DateTimeRange(d4, d1);
            var b = new DateTimeRange(d3, d2);

            a.Adjacent(b).ShouldBeFalse();
        }

        [TestMethod]
        [Description("Adjacent should correctly indicate when a.Start == b.Start")]
        public void Adjacent_should_correctly_indicate_when_starts_are_same()
        {
            var a = DateTime.Parse("15-Mar-2016");
            var b = DateTime.Parse("29-Mar-2016");
            var c = DateTime.Parse("15-Mar-2016");
            var d = DateTime.Parse("30-Mar-2016");

            var range1 = new DateTimeRange(a, b);
            var range2 = new DateTimeRange(c, d);

            range1.Adjacent(range2).ShouldBeFalse();

        }

        [TestMethod]
        [Description("Adjacent should correctly indicate when a.Start == b.End")]
        public void Adjacent_should_correctly_indicate_when_a_start_and_end_are_same()
        {
            var a = DateTime.Parse("15-Mar-2016");
            var b = DateTime.Parse("29-Mar-2016");
            var c = DateTime.Parse("10-Mar-2016");
            var d = DateTime.Parse("15-Mar-2016");

            var range1 = new DateTimeRange(a, b);
            var range2 = new DateTimeRange(c, d);

            range1.Adjacent(range2).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Adjacent should correctly indicate when a.End == b.Start")]
        public void Adjacent_should_correctly_indicate_when_end_and_start_are_same()
        {
            var a = DateTime.Parse("15-Mar-2016");
            var b = DateTime.Parse("20-Mar-2016");
            var c = DateTime.Parse("20-Mar-2016");
            var d = DateTime.Parse("25-Mar-2016");

            var range1 = new DateTimeRange(a, b);
            var range2 = new DateTimeRange(c, d);

            range1.Adjacent(range2).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Adjacent should correctly indicate when a.End == b.End")]
        public void Adjacent_should_correctly_when_ends_are_same()
        {
            var a = DateTime.Parse("15-Mar-2016");
            var b = DateTime.Parse("20-Mar-2016");
            var c = DateTime.Parse("10-Mar-2016");
            var d = DateTime.Parse("20-Mar-2016");

            var range1 = new DateTimeRange(a, b);
            var range2 = new DateTimeRange(c, d);

            range1.Adjacent(range2).ShouldBeFalse();
        }

        [TestMethod]
        [Description("Adjacent should correctly indicate when a.End == b.Start +- precision")]
        public void Adjacent_should_correctly_when_start_and_ends_diff_are_with_precision()
        {
            var a = DateTime.Parse("10-Mar-2016");
            var b = DateTime.Parse("15-Mar-2016 08:00");
            var c = DateTime.Parse("16-Mar-2016");
            var d = DateTime.Parse("20-Mar-2016");

            var range1 = new DateTimeRange(a, b);
            var range2 = new DateTimeRange(c, d);

            range1.Adjacent(range2, DateTimeUnits.Days).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Adjacent should correctly indicate when a.End == b.Start outside precision")]
        public void Adjacent_should_correctly_when_start_and_ends_diff_are_outside_precision()
        {
            var a = DateTime.Parse("10-Mar-2016");
            var b = DateTime.Parse("13-Mar-2016 08:00");
            var c = DateTime.Parse("16-Mar-2016");
            var d = DateTime.Parse("20-Mar-2016");

            var range1 = new DateTimeRange(a, b);
            var range2 = new DateTimeRange(c, d);

            range1.Adjacent(range2, DateTimeUnits.Days).ShouldBeFalse();
        }

        #endregion

        #region clone() Tests

        [TestMethod]
        [Description("Clone should deep clone range")]
        public void Clone_should_deep_clone_range()
        {
            var dr1 = new DateTimeRange(DateTimeOffset.Parse(sStart), DateTimeOffset.Parse(sEnd));
            var dr2 = dr1;

            dr2.Start.AddDays(2);
            dr1.Start.ShouldNotBeSameAs(dr2.Start);
        }



        #endregion

        #region by Tests

        [TestMethod]
        [Description("By should return a valid iterator")]
        public void By_should_return_a_valid_iterator()
        {
            var d1 = new DateTime(2012, 2, 1);
            var d2 = new DateTime(2012, 2, 5);
            var dr1 = new DateTimeRange(d1, d2);

            // Splat
            var i1 = dr1.By(DateTimeUnits.Days);
            i1.Count().ShouldBe(5);

            // For/of
            var i2 = dr1.By(DateTimeUnits.Days);
            var i = 0;
            foreach (var n in i2)
            {
                i++;
            }

            i.ShouldBe(5);

            // Array.from
            var i3 = dr1.By(DateTimeUnits.Days);

            i3.Count().ShouldBe(5);
        }


        [TestMethod]
        [Description("By_should iterate correctly by year over a Date-constructed range when leap years are involved")]
        public void By_should_iterate_correctly_by_year_over_a_DateConstructed_range_when_leap_years_are_involved()
        {
            var d1 = new DateTime(2011, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var d2 = new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var dr1 = new DateTimeRange(d1, d2);

            var i1 = dr1.By(DateTimeUnits.Years);

            var acc = i1.Select(m => m.ToUniversalTime().Year).ToList();

            acc.ShouldBe(new List<Int32> { 2011, 2012, 2013 });
        }

        [TestMethod]
        [Description("By should iterate correctly by year over a DateTime_constructed range when leap years are involved")]
        public void By_should_iterate_correctly_by_year_over_a_DateTimeParseConstructed_range_when_leap_years_are_involved()
        {
            var dr1 = new DateTimeRange(DateTime.Parse("2011-01-01"), DateTime.Parse("2013-01-01"));

            var i1 = dr1.By(DateTimeUnits.Years);


            var acc = i1.Select(m => m.Year);

            acc.ShouldBe(new List<Int32> { 2011, 2012, 2013 });
        }

        [TestMethod]
        [Description("By should iterate correctly by month over a DateTime_constructed range when leap years are involved")]
        public void By_should_iterate_correctly_by_month_over_a_DateTimeParse_Constructed_range_when_leap_years_are_involved()
        {
            var dr1 = new DateTimeRange(DateTime.ParseExact("2012-01", "yyyy-MM", CultureInfo.InvariantCulture), DateTime.ParseExact("2012-03", "yyyy-MM", CultureInfo.InvariantCulture));

            var i1 = dr1.By(DateTimeUnits.Months);



            var acc = i1.Select(m => m.ToUniversalTime().ToString("yyyy-MM"));

            acc.ShouldBe(new List<String> { "2012-01", "2012-02", "2012-03" });
        }

        [TestMethod]
        [Description("By should iterate correctly by month over a Date-contstructed range when leap years are involved")]
        public void By_should_iterate_correctly_by_month_over_a_DateContstructed_range_when_leap_years_are_involved()
        {
            var d1 = new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var d2 = new DateTime(2012, 3, 1, 0, 0, 0, DateTimeKind.Utc);
            var dr1 = new DateTimeRange(d1, d2);

            var i1 = dr1.By(DateTimeUnits.Months);



            var acc = i1.Select(m => m.ToUniversalTime().ToString("yyyy-MM"));

            acc.ShouldBe(new List<String> { "2012-01", "2012-02", "2012-03" });

        }

        [TestMethod]
        [Description("By should return a valid iterator")]
        public void By_should_return_empty_list_when_range_is_empty()
        {
            var dr1 = DateTimeRange.Empty;
            var milliseconds = dr1.By(DateTimeUnits.Milliseconds);
            var seconds = dr1.By(DateTimeUnits.Seconds);
            var minutes = dr1.By(DateTimeUnits.Minutes);
            var hours = dr1.By(DateTimeUnits.Hours);
            var days = dr1.By(DateTimeUnits.Days);
            var weeks = dr1.By(DateTimeUnits.Weeks);
            var months = dr1.By(DateTimeUnits.Months);
            var years = dr1.By(DateTimeUnits.Years);

            milliseconds.ShouldBeEmpty();
            seconds.ShouldBeEmpty();
            minutes.ShouldBeEmpty();
            hours.ShouldBeEmpty();
            days.ShouldBeEmpty();
            weeks.ShouldBeEmpty();
            months.ShouldBeEmpty();
            years.ShouldBeEmpty();
        }

        #endregion

        #region reverseBy Tests

        [TestMethod]
        [Description("reverseBy should return a valid iterator")]
        public void reverseBy_should_return_a_valid_iterator()
        {
            var d1 = new DateTime(2013, 2, 1, 0, 0, 0, DateTimeKind.Utc);
            var d2 = new DateTime(2013, 2, 5, 0, 0, 0, DateTimeKind.Utc);
            var dr1 = new DateTimeRange(d1, d2);

            // Splat
            var i1 = dr1.ByReverse(DateTimeUnits.Days);
            i1.Count().ShouldBe(5);

            // For/of
            var i2 = dr1.ByReverse(DateTimeUnits.Days);
            var i = 0;
            foreach (var n in i2)
            {
                i++;
            }

            i.ShouldBe(5);

            // Array.from
            var i3 = dr1.ByReverse(DateTimeUnits.Days);
            var acc = i3;
            acc.Count().ShouldBe(5);
        }

        [TestMethod]
        [Description("reverseBy should iterate correctly by shorthand string")]
        public void reverseBy_should_iterate_correctly_by_shorthand_string()
        {
            var d1 = new DateTime(2013, 2, 1, 0, 0, 0, DateTimeKind.Utc);
            var d2 = new DateTime(2013, 2, 5, 0, 0, 0, DateTimeKind.Utc);
            var dr1 = new DateTimeRange(d1, d2);

            var i1 = dr1.ByReverse(DateTimeUnits.Days);
            var acc = i1.ToList();

            acc.Count().ShouldBe(5);
            acc[0].ToUniversalTime().Day.ShouldBe(5);
            acc[1].ToUniversalTime().Day.ShouldBe(4);
            acc[2].ToUniversalTime().Day.ShouldBe(3);
            acc[3].ToUniversalTime().Day.ShouldBe(2);
            acc[4].ToUniversalTime().Day.ShouldBe(1);
        }

        [TestMethod]
        [Description("Reverse By should iterate correctly by year over a Date-constructed range when leap years are involved")]
        public void reverseBy_should_iterate_correctly_by_year_over_a_DateConstructed_range_when_leap_years_are_involved()
        {
            var d1 = new DateTime(2011, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var d2 = new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var dr1 = new DateTimeRange(d1, d2);

            var i1 = dr1.ByReverse(DateTimeUnits.Years);
            var acc = i1.Select(m => m.ToUniversalTime().Year);

            acc.ShouldBe(new List<Int32> { 2013, 2012, 2011 });
        }


        [TestMethod]
        [Description("Reverse By should iterate correctly by year over a DateTime_constructed range when leap years are involved")]
        public void reverseBy_should_iterate_correctly_by_year_over_a_DateTimeParseConstructed_range_when_leap_years_are_involved()
        {
            var dr1 = new DateTimeRange(DateTime.ParseExact("2011-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.ParseExact("2013-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture));
            var i1 = dr1.ByReverse(DateTimeUnits.Years);
            var acc = i1.Select(m => m.Year);
            acc.ShouldBe(new List<Int32> { 2013, 2012, 2011 });
        }

        [TestMethod]
        [Description("reverseBy should iterate correctly by month over a DateTime_constructed range when leap years are involved")]
        public void reverseBy_should_iterate_correctly_by_month_over_a_DateTime_constructed_range_when_leap_years_are_involved()
        {
            var dr1 = new DateTimeRange(DateTime.ParseExact("2012-01", "yyyy-MM", CultureInfo.InvariantCulture), DateTime.ParseExact("2012-03", "yyyy-MM", CultureInfo.InvariantCulture));

            var i1 = dr1.ByReverse(DateTimeUnits.Months);
            var acc = i1.Select(m => m.ToUniversalTime().ToString("yyyy-MM"));

            acc.ShouldBe(new List<String> { "2012-03", "2012-02", "2012-01" });
        }

        [TestMethod]
        [Description("should iterate correctly by month over a Date-contstructed range when leap years are involved")]
        public void reverseBy_should_iterate_correctly_by_month_over_a_DateContstructed_range_when_leap_years_are_involved()
        {
            var d1 = new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var d2 = new DateTime(2012, 3, 28, 0, 0, 0, DateTimeKind.Utc);
            var dr1 = new DateTimeRange(d1, d2);

            var i1 = dr1.ByReverse(DateTimeUnits.Months);
            var acc = i1.Select(m => m.ToUniversalTime().ToString("yyyy-MM"));

            acc.ShouldBe(new List<String> { "2012-03", "2012-02", "2012-01" });
        }


        [TestMethod]
        [Description("reverseBy should correctly iterate by a given step")]
        public void reverseBy_should_correctly_iterate_by_a_given_step()
        {
            var my1 = DateTimeOffset.Parse("2014-04-02T00:00:00.000Z");
            var my2 = DateTimeOffset.Parse("2014-04-08T00:00:00.000Z");
            var dr1 = new DateTimeRange(my1, my2);

            var acc = dr1.ByReverse(DateTimeUnits.Days, step: 2).Select(m => m.ToUniversalTime().ToString("dd"));
            acc.ShouldBe(new List<String> { "08", "06", "04", "02" });
        }


        [TestMethod]
        [Description("ByReverse should return a valid iterator")]
        public void ByReverse_should_return_empty_list_when_range_is_empty()
        {
            var dr1 = DateTimeRange.Empty;
            var milliseconds = dr1.ByReverse(DateTimeUnits.Milliseconds);
            var seconds = dr1.ByReverse(DateTimeUnits.Seconds);
            var minutes = dr1.ByReverse(DateTimeUnits.Minutes);
            var hours = dr1.ByReverse(DateTimeUnits.Hours);
            var days = dr1.ByReverse(DateTimeUnits.Days);
            var weeks = dr1.ByReverse(DateTimeUnits.Weeks);
            var months = dr1.ByReverse(DateTimeUnits.Months);
            var years = dr1.ByReverse(DateTimeUnits.Years);

            milliseconds.ShouldBeEmpty();
            seconds.ShouldBeEmpty();
            minutes.ShouldBeEmpty();
            hours.ShouldBeEmpty();
            days.ShouldBeEmpty();
            weeks.ShouldBeEmpty();
            months.ShouldBeEmpty();
            years.ShouldBeEmpty();
        }

        #endregion

        #region byRange Tests

        //        [TestMethod]
        //[Description("should return a valid iterator")]
        //        public void should_return_a_valid_iterator()
        //        {
        //            var d1 = new DateTime(2012, 2, 1, 0, 0, 0, DateTimeKind.Utc);
        //            var d2 = new DateTime(2012, 2, 5, 0, 0, 0, DateTimeKind.Utc);
        //            var d3 = new DateTime(2012, 2, 15, 0, 0, 0, DateTimeKind.Utc);
        //            var d4 = new DateTime(2012, 2, 16, 0, 0, 0, DateTimeKind.Utc);
        //            var dr1 = new DateTimeRange(d1, d2);
        //            var dr2 = new DateTimeRange(d3, d4);

        //            // Splat
        //            var i1 = dr1.by (dr2);
        //             [...i1].Count().ShouldBe(5);
        //)

        //             // For/of
        //             var i2 = dr1.byRange(dr2);
        //        let i = 0;
        //             for (let n of i2) {
        //               i++;
        //             }
        //    i).ShouldBe(5);
        //)

        //             // Array.from
        //             var i3 = dr1.byRange(dr2);
        //    var acc = i3);
        //acc.Count().ShouldBe(5);
        //)
        //        //   });

        //    //   [TestMethod]
        //[Description("should iterate correctly by range")]
        //public void should_iterate_correctly_by_range()
        //    {
        //        //     var d1 = new DateTime(2012, 2, 1, 0, 0 ,0 , DateTimeKind.Utc);
        //        //     var d2 = new DateTime(2012, 2, 5, 0, 0 ,0 , DateTimeKind.Utc);
        //        //     var dr1 = new DateTimeRange(d1, d2);
        //        //     var dr2 = new DateTimeRange(1000 * 60 * 60 * 24);

        //        //     var acc = dr1.byRange(dr2));

        //        //     acc.Count().ShouldBe(5);
        //)
        //        //     acc[0].ToUniversalTime().Day).ShouldBe(1);
        //)
        //        //     acc[1].ToUniversalTime().Day).ShouldBe(2);
        //)
        //        //     acc[2].ToUniversalTime().Day).ShouldBe(3);
        //)
        //        //     acc[3].ToUniversalTime().Day).ShouldBe(4);
        //)
        //        //     acc[4].ToUniversalTime().Day).ShouldBe(5);
        //)
        //        //   });

        //        //   [TestMethod]
        //[Description("should iterate correctly by duration")]
        //    public void should_iterate_correctly_by_duration()
        //    {
        //        //     var d1 = new DateTime(2014, 9, 6, 0, 0, 0, 0 ,0 , DateTimeKind.Utc);
        //        //     var d2 = new DateTime(2014, 9, 6, 23, 59, 0, 0 ,0 , DateTimeKind.Utc);
        //        //     var dr1 = new DateTimeRange(d1, d2);
        //        //     var dr2 = moment.duration(15, "minutes");

        //        //     var acc = dr1.byRange(dr2));

        //        //     acc.Count().ShouldBe(96);
        //)
        //        //     acc[0].minute()).ShouldBe(0);
        //)
        //        //     acc[95].minute()).ShouldBe(45);
        //)
        //        //   });

        //        //   [TestMethod]
        //[Description("should not include .End in the iteration if exclusive is set to true when iterating by range")]
        //    public void should_not_include.End_in_the_iteration_if_exclusive_is_set_to_true_when_iterating_by_range()
        //    {
        //        //     var my1 = DateTime.Parse("2014-04-02T00:00:00.000Z");
        //        //     var my2 = DateTime.Parse("2014-04-04T00:00:00.000Z");
        //        //     var dr1 = new DateTimeRange(my1, my2);
        //        //     var dr2 = new DateTimeRange(my1, DateTime.Parse("2014-04-03T00:00:00.000Z"));
        //        //     let acc;

        //        //     acc = dr1.byRange(dr2)).Select(m => m.ToUniversalTime().ToString("yyyy-MM-dd"));
        //        //     acc.ShouldBe(new List<>{"2014-04-02", "2014-04-03", "2014-04-04"});
        //)

        //        //     acc = dr1.byRange(dr2, { exclusive: false, step:1 })).Select(m => m.ToUniversalTime().ToString("yyyy-MM-dd"));
        //        //     acc.ShouldBe(new List<>{"2014-04-02", "2014-04-03", "2014-04-04"});
        //)

        //        //     acc = dr1.byRange(dr2, { exclusive: true, step:1 })).Select(m => m.ToUniversalTime().ToString("yyyy-MM-dd"));
        //        //     acc.ShouldBe(new List<>{"2014-04-02", "2014-04-03"});
        //)
        //        //   });

        //        //   [TestMethod]
        //[Description("should iterate correctly by a given step")]
        //    public void should_iterate_correctly_by_a_given_step()
        //    {
        //        //     var d1 = new DateTime(2012, 2, 2, 0, 0 ,0 , DateTimeKind.Utc);
        //        //     var d2 = new DateTime(2012, 2, 6, 0, 0 ,0 , DateTimeKind.Utc);
        //        //     var dr1 = new DateTimeRange(d1, d2);
        //        //     var dr2 = moment.duration(1000 * 60 * 60 * 24);

        //        //     var acc = dr1.byRange(dr2, { step: 2, exclusive:false })).Select(m => m.ToUniversalTime().ToString("DD"));

        //        //     acc.ShouldBe(new List<>{"02", "04", "06"});
        //)
        //        //   });

        //        //   [TestMethod]
        //[Description("should iterate correctly by a given step when exclusive")]
        //    public void should_iterate_correctly_by_a_given_step_when_exclusive()
        //    {
        //        //     var d1 = new DateTime(2012, 2, 2, 0, 0 ,0 , DateTimeKind.Utc);
        //        //     var d2 = new DateTime(2012, 2, 6, 0, 0 ,0 , DateTimeKind.Utc);
        //        //     var dr1 = new DateTimeRange(d1, d2);
        //        //     var dr2 = 1000 * 60 * 60 * 24;

        //        //     var acc = dr1.byRange(dr2, { exclusive: true, step: 2 })).Select(m => m.ToUniversalTime().ToString("DD"));

        //        //     acc.ShouldBe(new List<>{"02", "04"});
        //)
        //        //   });
        //        // });

        #endregion

        #region reverseByRange Tests

        //   [TestMethod]
        //[Description("should return a valid iterator")]












        //public void should_return_a_valid_iterator()
        //{
        //     var d1 = new DateTime(2012, 2, 1, 0, 0 ,0 , DateTimeKind.Utc);
        //     var d2 = new DateTime(2012, 2, 5, 0, 0 ,0 , DateTimeKind.Utc);
        //     var d3 = new DateTime(2012, 2, 15, 0, 0 ,0 , DateTimeKind.Utc);
        //     var d4 = new DateTime(2012, 2, 16, 0, 0 ,0 , DateTimeKind.Utc);
        //     var dr1 = new DateTimeRange(d1, d2);
        //     var dr2 = new DateTimeRange(d3, d4);

        //     // Splat
        //     var i1 = dr1.ByReverseRange(dr2);
        //     [...i1].Count().ShouldBe(5);

        //     // For/of
        //     var i2 = dr1.ByReverseRange(dr2);
        //     let i = 0;
        //     for (let n of i2) {
        //       i++;
        //     }
        //     i).ShouldBe(5);

        //     // Array.from
        //     var i3 = dr1.ByReverseRange(dr2);
        //     var acc = i3);
        //     acc.Count().ShouldBe(5);
        //   });

        //   [TestMethod]
        //[Description("should iterate correctly by range")]
        //public void should_iterate_correctly_by_range()
        //{
        //     var d1 = new DateTime(2012, 2, 1, 0, 0 ,0 , DateTimeKind.Utc);
        //     var d2 = new DateTime(2012, 2, 5, 0, 0 ,0 , DateTimeKind.Utc);
        //     var dr1 = new DateTimeRange(d1, d2);
        //     var dr2 = 1000 * 60 * 60 * 24;

        //     var acc = dr1.ByReverseRange(dr2));

        //     acc.Count().ShouldBe(5);
        //     acc[0].ToUniversalTime().Day).ShouldBe(5);
        //     acc[1].ToUniversalTime().Day).ShouldBe(4);
        //     acc[2].ToUniversalTime().Day).ShouldBe(3);
        //     acc[3].ToUniversalTime().Day).ShouldBe(2);
        //     acc[4].ToUniversalTime().Day).ShouldBe(1);
        //   });

        //   [TestMethod]
        //[Description("should iterate correctly by duration")]
        //public void should_iterate_correctly_by_duration()
        //{
        //     var d1 = new DateTime(2014, 9, 6, 0, 1, 0, 0 ,0 , DateTimeKind.Utc);
        //     var d2 = new DateTime(2014, 9, 7, 0, 0, 0, 0 ,0 , DateTimeKind.Utc);
        //     var dr1 = new DateTimeRange(d1, d2);
        //     var dr2 = moment.duration(15, "minutes");

        //     var acc = dr1.ByReverseRange(dr2));

        //     acc.Count().ShouldBe(96);
        //     acc[0].minute()).ShouldBe(0);
        //     acc[95].minute()).ShouldBe(15);
        //   });

        //   [TestMethod]
        //[Description("should not include .Start in the iteration if exclusive is set to true when iterating by range")]
        //public void should_not_include.Start_in_the_iteration_if_exclusive_is_set_to_true_when_iterating_by_range()
        //{
        //     var my1 = DateTime.Parse("2014-04-02T00:00:00.000Z");
        //     var my2 = DateTime.Parse("2014-04-04T00:00:00.000Z");
        //     var dr1 = new DateTimeRange(my1, my2);
        //     var dr2 = new DateTimeRange(my1, DateTime.Parse("2014-04-03T00:00:00.000Z"));
        //     let acc;

        //     acc = dr1.ByReverseRange(dr2)).Select(m => m.ToUniversalTime().ToString("yyyy-MM-dd"));
        //     acc.ShouldBe(new List<>{"2014-04-04", "2014-04-03", "2014-04-02"});

        //     acc = dr1.ByReverseRange(dr2, { exclusive: false, step:1 })).Select(m => m.ToUniversalTime().ToString("yyyy-MM-dd"));
        //     acc.ShouldBe(new List<>{"2014-04-04", "2014-04-03", "2014-04-02"});

        //     acc = dr1.ByReverseRange(dr2, { exclusive: true, step:1 })).Select(m => m.ToUniversalTime().ToString("yyyy-MM-dd"));
        //     acc.ShouldBe(new List<>{"2014-04-04", "2014-04-03"});
        //   });

        //   [TestMethod]
        //[Description("should iterate correctly by a given step")]
        //public void should_iterate_correctly_by_a_given_step()
        //{
        //     var d1 = new DateTime(2012, 2, 2, 0, 0 ,0 , DateTimeKind.Utc);
        //     var d2 = new DateTime(2012, 2, 6, 0, 0 ,0 , DateTimeKind.Utc);
        //     var dr1 = new DateTimeRange(d1, d2);
        //     var dr2 = 1000 * 60 * 60 * 24;

        //     var acc = dr1.ByReverseRange(dr2, { step: 2, exclusive:false })).Select(m => m.ToUniversalTime().ToString("DD"));

        //     acc.ShouldBe(new List<>{"06", "04", "02"});
        //   });

        //   [TestMethod]
        //[Description("should iterate correctly by a given step when exclusive")]
        //public void should_iterate_correctly_by_a_given_step_when_exclusive()
        //{
        //     var d1 = new DateTime(2012, 2, 2, 0, 0 ,0 , DateTimeKind.Utc);
        //     var d2 = new DateTime(2012, 2, 6, 0, 0 ,0 , DateTimeKind.Utc);
        //     var dr1 = new DateTimeRange(d1, d2);
        //     var dr2 = 1000 * 60 * 60 * 24;

        //     var acc = dr1.ByReverseRange(dr2, { exclusive: true, step: 2 })).Select(m => m.ToUniversalTime().ToString("DD"));

        //     acc.ShouldBe(new List<>{"06", "04"});
        //   });
        // });

        #endregion

        #region contains() Tests


        [TestMethod]
        [Description("Contains should work with Date objects")]
        public void Contains_should_work_with_Date_objects()
        {
            var dr = new DateTimeRange(d1, d2);

            dr.Contains(d3).ShouldBeTrue();
            dr.Contains(d4).ShouldBeFalse();
        }

        [TestMethod]
        [Description("Contains should work with Moment objects")]
        public void Contains_should_work_with_Moment_objects()
        {
            var dr = new DateTimeRange(m1, m2);

            dr.Contains(m3).ShouldBeTrue();

            dr.Contains(m4).ShouldBeFalse();

        }

        [TestMethod]
        [Description("Contains should work with DateRange objects")]
        public void Contains_should_work_with_DateRange_objects()
        {
            var dr1 = new DateTimeRange(m1, m4);
            var dr2 = new DateTimeRange(m3, m2);

            dr1.Contains(dr2).ShouldBeTrue();
            dr2.Contains(dr1).ShouldBeFalse();

        }

        [TestMethod]
        [Description("Contains should be an inclusive comparison")]
        public void Contains_should_be_an_inclusive_comparison()
        {
            var dr1 = new DateTimeRange(m1, m4);

            dr1.Contains(m1).ShouldBeTrue();
            dr1.Contains(m4).ShouldBeTrue();
            dr1.Contains(dr1).ShouldBeTrue();
        }





        #endregion

        #region overlaps() Tests

        [TestMethod]
        [Description("Overlaps should work with DateTime objects")]
        public void Overlaps_should_work_with_DateRange_objects()
        {
            var dr1 = new DateTimeRange(m1, m2);
            var dr2 = new DateTimeRange(m3, m4);
            var dr3 = new DateTimeRange(m2, m4);
            var dr4 = new DateTimeRange(m1, m3);

            dr1.Overlaps(dr2).ShouldBeTrue();
            dr1.Overlaps(dr3).ShouldBeTrue();

            dr4.Overlaps(dr3).ShouldBeFalse();
        }


        [TestMethod]
        [Description("Overlaps should consider ranges overlaping if partially overlapped")]
        public void Overlaps_should_consider_ranges_overlaping_if_partially_overlapped()
        {
            var a = DateTime.Parse("15-Mar-2016");
            var b = DateTime.Parse("18-Mar-2016");
            var c = DateTime.Parse("20-Mar-2016");
            var d = DateTime.Parse("25-Mar-2016");

            var range1 = new DateTimeRange(a, c);
            var range2 = new DateTimeRange(b, d);

            range1.Overlaps(range2).ShouldBeTrue();


        }




        #endregion

        #region intersect() Tests


        [TestMethod]
        [Description("Intersect should work with [---{==]---} overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_overlaps_where_a_inclusive_b_exclusive()
        {
            var dr1 = new DateTimeRange(d5, d7);
            var dr2 = new DateTimeRange(d6, d8);

            dr1.Intersection(dr2).IsSame(new DateTimeRange(d6, d7)).ShouldBeTrue();
        }

        [TestMethod]
        [Description("should work with {---[==}---] overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_overlaps_where5()
        {
            var dr1 = new DateTimeRange(d6, d8);
            var dr2 = new DateTimeRange(d5, d7);

            dr1.Intersection(dr2).IsSame(new DateTimeRange(d6, d7)).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Intersect_should work with [{===]---} overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_overlaps_where_a_b_()
        {
            var dr1 = new DateTimeRange(d5, d6);
            var dr2 = new DateTimeRange(d5, d7);

            dr1.Intersection(dr2).IsSame(new DateTimeRange(d5, d6)).ShouldBeTrue();

        }

        [TestMethod]
        [Description("Intersect should work with {[===}---] overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_overlaps_wherea_b()
        {
            var dr1 = new DateTimeRange(d5, d7);
            var dr2 = new DateTimeRange(d5, d6);

            dr1.Intersection(dr2).IsSame(new DateTimeRange(d5, d6)).ShouldBeTrue();

        }

        [TestMethod]
        [Description("Intersect should work with [---{===]} overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_overlaps_where_a_b()
        {
            var dr1 = new DateTimeRange(d5, d7);
            var dr2 = new DateTimeRange(d6, d7);

            dr1.Intersection(dr2).IsSame(new DateTimeRange(d6, d7)).ShouldBeTrue();

        }

        [TestMethod]
        [Description("Intersect should work with {---[===}] overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_dateRange_overlaps_where()
        {
            var dr1 = new DateTimeRange(d6, d7);
            var dr2 = new DateTimeRange(d5, d7);

            dr1.Intersection(dr2).IsSame(new DateTimeRange(d6, d7)).ShouldBeTrue();

        }

        [TestMethod]
        [Description("Intersect should work with [---] {---} overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_dateRange_overlaps_where_a_b()
        {
            var dr1 = new DateTimeRange(d5, d6);
            var dr2 = new DateTimeRange(d7, d8);

            dr1.Intersection(dr2).ShouldBe(DateTimeRange.Empty);
        }

        [TestMethod]
        [Description("Intersect should work with {---} [---] overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_overlaps_where()
        {
            var dr1 = new DateTimeRange(d7, d8);
            var dr2 = new DateTimeRange(d5, d6);

            dr1.Intersection(dr2).ShouldBe(DateTimeRange.Empty);

        }

        [TestMethod]
        [Description("Intersect should work with [---]{---} overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_DateTimeRange_overlaps_where_a_b()
        {
            var dr1 = new DateTimeRange(d5, d6);
            var dr2 = new DateTimeRange(d6, d7);

            dr1.Intersection(dr2).ShouldBe(new DateTimeRange(d6, d6));

        }

        [TestMethod]
        [Description("Intersect should work with {---}[---] overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_overlaps_where_a()
        {
            var dr1 = new DateTimeRange(d6, d7);
            var dr2 = new DateTimeRange(d5, d6);
            dr1.Intersection(dr2).ShouldBe(new DateTimeRange(d6, d6));

        }


        [TestMethod]
        [Description("should work with {--[===]--} overlaps where (a=[], b={})")]
        public void should_work_with_overlaps_where1()
        {
            var dr1 = new DateTimeRange(d6, d7);
            var dr2 = new DateTimeRange(d5, d8);

            dr1.Intersection(dr2).IsSame(dr1).ShouldBeTrue();

        }

        [TestMethod]
        [Description("Intersect should work with [--{===}--] overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_overlaps_where_a_b2()
        {
            var dr1 = new DateTimeRange(d5, d8);
            var dr2 = new DateTimeRange(d6, d7);

            dr1.Intersection(dr2).IsSame(dr2).ShouldBeTrue();

        }

        [TestMethod]
        [Description("should work with [{===}] overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_overlaps_where_a_b3()
        {
            var dr1 = new DateTimeRange(d5, d6);
            var dr2 = new DateTimeRange(d5, d6);

            dr1.Intersection(dr2).IsSame(dr2).ShouldBeTrue();

        }

        [TestMethod]
        [Description("Intersect should work with [--{}--] overlaps where (a=[], b={})")]
        public void Intersect_should_work_with_overlaps_where_a_b4()
        {
            var dr1 = new DateTimeRange(d6, d6);
            var dr2 = new DateTimeRange(d5, d7);

            dr1.Intersection(dr2).IsSame(dr1).ShouldBeTrue();


        }



        #endregion

        #region add() Tests


        [TestMethod]
        [Description("Add should add ranges with [---{==]---} overlaps where (a=[], b={})")]
        public void Add_should_add_ranges_with_overlaps_where_a_b()
        {
            var dr1 = new DateTimeRange(d5, d7);
            var dr2 = new DateTimeRange(d6, d8);

            dr1.Add(dr2).IsSame(new DateTimeRange(d5, d8)).ShouldBeTrue();

        }

        [TestMethod]
        [Description("Add should add ranges with {---[==}---] overlaps where (a=[], b={})")]
        public void Add_should_add_ranges_with__overlaps_where_a_b()
        {
            var dr1 = new DateTimeRange(d6, d8);
            var dr2 = new DateTimeRange(d5, d7);

            dr1.Add(dr2).IsSame(new DateTimeRange(d5, d8)).ShouldBeTrue();

        }

        [TestMethod]
        [Description("Add should add ranges with [{===]---} overlaps where (a=[], b={})")]
        public void Add_should_add_ranges_with_datetimeRange_overlaps_where_a_b()
        {
            var dr1 = new DateTimeRange(d5, d6);
            var dr2 = new DateTimeRange(d5, d7);

            dr1.Add(dr2).IsSame(new DateTimeRange(d5, d7)).ShouldBeTrue();

        }

        [TestMethod]
        [Description("Add should add ranges with {[===}---] overlaps where (a=[], b={})")]
        public void Add_should_add_ranges_with_datetimerange_overlaps_where_a_b()
        {
            var dr1 = new DateTimeRange(d5, d7);
            var dr2 = new DateTimeRange(d5, d6);

            dr1.Add(dr2).IsSame(new DateTimeRange(d5, d7)).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Add should add ranges with [---{===]} overlaps where (a=[], b={})")]
        public void Add_should_add_ranges_with_overlaps_where_a_b3()
        {
            var dr1 = new DateTimeRange(d5, d7);
            var dr2 = new DateTimeRange(d6, d7);

            dr1.Add(dr2).IsSame(new DateTimeRange(d5, d7)).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Add should add ranges with {---[===}] overlaps where (a=[], b={})")]
        public void Add_should_add_ranges_with_datetimerange_overlaps_where_a_b2()
        {
            var dr1 = new DateTimeRange(d6, d7);
            var dr2 = new DateTimeRange(d5, d7);

            dr1.Add(dr2).IsSame(new DateTimeRange(d5, d7)).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Add should not add ranges with [---] {---} overlaps where (a=[], b={})")]
        public void Add_should_not_add_ranges_with_overlaps_where_a_b4()
        {
            var dr1 = new DateTimeRange(d5, d6);
            var dr2 = new DateTimeRange(d7, d8);

            dr1.Add(dr2).ShouldBe(DateTimeRange.Empty);
        }

        [TestMethod]
        [Description("Add should not add ranges with {---} [---] overlaps where (a=[], b={})")]
        public void Add_should_not_add_ranges_with_overlaps_where_a_b5()
        {
            var dr1 = new DateTimeRange(d7, d8);
            var dr2 = new DateTimeRange(d5, d6);

            dr1.Add(dr2).ShouldBe(DateTimeRange.Empty);
        }

        [TestMethod]
        [Description("Add should add ranges with [---]{---} overlaps where (a=[], b={})")]
        public void Add_should_not_add_ranges_with_adjacent_overlaps_where_a_b6()
        {
            var dr1 = new DateTimeRange(d5, d6);
            var dr2 = new DateTimeRange(d6, d7);

            dr1.Add(dr2).ShouldBe(new DateTimeRange(d5, d7));
        }

        [TestMethod]
        [Description("Add should not add ranges with {---}[---] overlaps where (a=[], b={})")]
        public void Add_should_not_add_ranges_adjecent_with_overlaps_where_a_b7()
        {
            var dr1 = new DateTimeRange(d6, d7);
            var dr2 = new DateTimeRange(d5, d6);

            dr1.Add(dr2).ShouldBe(new DateTimeRange(d5, d7));
        }

        [TestMethod]
        [Description("Add should add ranges {--[===]--} overlaps where (a=[], b={})")]
        public void Add_should_add_ranges_overlaps_where_a_b8()
        {
            var dr1 = new DateTimeRange(d6, d7);
            var dr2 = new DateTimeRange(d5, d8);

            dr1.Add(dr2).IsSame(new DateTimeRange(d5, d8)).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Add should add ranges [--{===}--] overlaps where (a=[], b={})")]
        public void Add_should_add_ranges_overlaps_where_a_b9()
        {
            var dr1 = new DateTimeRange(d5, d8);
            var dr2 = new DateTimeRange(d6, d7);

            dr1.Add(dr2).IsSame(new DateTimeRange(d5, d8)).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Add should add ranges [{===}] overlaps where (a=[], b={})")]
        public void Add_should_add_ranges_overlaps_where_a_b10()
        {
            var dr1 = new DateTimeRange(d5, d6);
            var dr2 = new DateTimeRange(d5, d6);

            dr1.Add(dr2).IsSame(new DateTimeRange(d5, d6)).ShouldBeTrue();
        }



        #endregion

        #region subtract() Tests


        [TestMethod]
        [Description("Subtract should turn [--{==}--] into (--) (--) where (a=[], b={})")]
        public void Subtract_should_turn_addition_into_where_a_b()
        {
            var dr1 = new DateTimeRange(d5, d8);
            var dr2 = new DateTimeRange(d6, d7);
            var result = dr1.Subtract(dr2);

            result.Item1.HasValue.ShouldBeTrue();
            result.Item2.HasValue.ShouldBeTrue();

            result.Item1?.IsEqual(new DateTimeRange(d5, d6)).ShouldBeTrue();
            result.Item2?.IsEqual(new DateTimeRange(d7, d8)).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Subtract should turn {--[==]--} into () where (a=[], b={})")]
        public void Subtract_should_turn_into_where__a_b()
        {
            var dr1 = new DateTimeRange(d6, d7);
            var dr2 = new DateTimeRange(d5, d8);

            var result = dr1.Subtract(dr2);
            result.Item1.HasValue.ShouldBeFalse();
            result.Item2.HasValue.ShouldBeFalse();
        }

        [TestMethod]
        [Description("Subtract should turn {[==]} into () where (a=[], b={})")]
        public void Subtract_should_turn_into_empty_where_a_b()
        {
            var dr1 = new DateTimeRange(d5, d6);
            var dr2 = new DateTimeRange(d5, d6);

            var result = dr1.Subtract(dr2);
            result.Item1.HasValue.ShouldBeFalse();
            result.Item2.HasValue.ShouldBeFalse();
            ;
        }

        [TestMethod]
        [Description("Subtract should turn [--{==]--} into (--) where (a=[], b={})")]
        public void Subtract_should_turn__into_where_a_b()
        {
            var dr1 = new DateTimeRange(d5, d7);
            var dr2 = new DateTimeRange(d6, d8);
            var result = dr1.Subtract(dr2);

            result.Item1.HasValue.ShouldBeTrue();
            result.Item2.HasValue.ShouldBeFalse();
            ;
            result.Item1?.IsEqual(new DateTimeRange(d5, d6)).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Subtract should turn [--{==]} into (--) where (a=[], b={})")]
        public void Subtract_should_turn__into_where_a_b2()
        {
            var dr1 = new DateTimeRange(d5, d7);
            var dr2 = new DateTimeRange(d6, d7);

            var result = dr1.Subtract(dr2);
            result.Item1.HasValue.ShouldBeTrue();
            result.Item2.HasValue.ShouldBeFalse();
            result.Item1?.IsEqual(new DateTimeRange(d5, d6)).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Subtract should turn {--[==}--] into (--) where (a=[], b={})")]
        public void Subtract_should_turn__into_where_a_b3()
        {
            var dr1 = new DateTimeRange(d6, d8);
            var dr2 = new DateTimeRange(d5, d7);

            var result = dr1.Subtract(dr2);
            result.Item1.HasValue.ShouldBeTrue();
            result.Item2.HasValue.ShouldBeFalse();
            result.Item1?.IsEqual(new DateTimeRange(d7, d8)).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Subtract should turn {[==}--] into (--) where (a=[], b={})")]
        public void Subtract_should_turn__into__where_a_b4()
        {
            var dr1 = new DateTimeRange(d6, d8);
            var dr2 = new DateTimeRange(d6, d7);
            var result = dr1.Subtract(dr2);
            result.Item1.HasValue.ShouldBeTrue();
            result.Item2.HasValue.ShouldBeFalse();
            result.Item1?.IsEqual(new DateTimeRange(d7, d8)).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Subtract should turn [--] {--} into (--) where (a=[], b={})")]
        public void Subtract_should_turn__into__where_a_b5()
        {
            var dr1 = new DateTimeRange(d5, d6);
            var dr2 = new DateTimeRange(d7, d8);

            var result = dr1.Subtract(dr2);
            result.Item1.HasValue.ShouldBeTrue();
            result.Item2.HasValue.ShouldBeFalse();
        }

        [TestMethod]
        [Description("Subtract should turn {--} [--] into (--) where (a=[], b={})")]
        public void Subtract_should_turn__into_where__a_b()
        {
            var dr1 = new DateTimeRange(d7, d8);
            var dr2 = new DateTimeRange(d5, d6);

            var result = dr1.Subtract(dr2);
            result.Item1.HasValue.ShouldBeTrue();
            result.Item2.HasValue.ShouldBeFalse();
            result.Item1?.ShouldBe(dr1);
        }

        [TestMethod]
        [Description("Subtract should turn [--{==}--] into (--) where (a=[], b={})")]
        public void Subtract_should_turn__into__where_a_b()
        {
            var o = new DateTimeRange(DateTime.Parse("2015-04-07T00:00:00+00:00"), DateTime.Parse("2015-04-08T00:00:00+00:00"));
            var s = new DateTimeRange(DateTime.Parse("2015-04-07T17:12:18+00:00"), DateTime.Parse("2015-04-07T17:12:18+00:00"));
            var subtraction = o.Subtract(s);
            var a = new DateTimeRange(DateTime.Parse("2015-04-07T00:00:00+00:00"), DateTime.Parse("2015-04-07T17:12:18+00:00"));
            var b = new DateTimeRange(DateTime.Parse("2015-04-07T17:12:18+00:00"), DateTime.Parse("2015-04-08T00:00:00+00:00"));


            subtraction.Item1?.Start.IsSame(a.Start).ShouldBeTrue();

            subtraction.Item1?.End.IsSame(a.End).ShouldBeTrue();

            subtraction.Item2?.Start.IsSame(b.Start).ShouldBeTrue();

            subtraction.Item2?.End.IsSame(b.End).ShouldBeTrue();
        }





        #endregion

        #region IsSame() Tests

        [TestMethod]
        [Description("IsSame should true if the start and end of both DateRange objects equal")]
        public void IsSame_should_true_if_the_start_and_end_of_both_DateRange_objects_equal()
        {
            var dr1 = new DateTimeRange(d1, d2);
            var dr2 = new DateTimeRange(d1, d2);

            dr1.IsSame(dr2).ShouldBeTrue();
        }

        [TestMethod]
        [Description("IsSame should false if the starts differ between objects")]
        public void IsSame_should_false_if_the_starts_differ_between_objects()
        {
            var dr1 = new DateTimeRange(d1, d3);
            var dr2 = new DateTimeRange(d2, d3);

            dr1.IsSame(dr2).ShouldBeFalse();
        }

        [TestMethod]
        [Description("IsSame_should false if the ends differ between objects")]
        public void should_false_if_the_ends_differ_between_objects()
        {
            var dr1 = new DateTimeRange(d1, d2);
            var dr2 = new DateTimeRange(d1, d3);

            dr1.IsSame(dr2).ShouldBeFalse();
        }






        #endregion

        #region toString() Tests

        [TestMethod]
        [Description("toString should be a correctly formatted ISO8601 Time Interval")]
        public void should_be_a_correctly_formatted_ISO8601_Time_Interval()
        {
            var start = DateTime.Parse("2015-01-17T09:50:04+00:00");
            var end = DateTime.Parse("2015-04-17T08:29:55+00:00");
            var dr = new DateTimeRange(start, end);

            dr.ToString().ShouldBe(start.ToString("s") + "/" + end.ToString("s"));

        }



        #endregion

        #region valueOf() Tests

        [TestMethod]
        [Description("valueOf should be the value of the range in milliseconds")]
        public void should_be_the_value_of_the_range_in_milliseconds()
        {
            var dr = new DateTimeRange(d1, d2);

            dr.Duration.TotalMilliseconds.ShouldBe(TimeSpan.FromTicks(d2.Ticks - d1.Ticks).TotalMilliseconds);
            dr.Duration.ShouldBe(TimeSpan.FromTicks(d2.Ticks - d1.Ticks));
        }

        [TestMethod]
        [Description("should correctly coerce to a number")]
        public void should_correctly_coerce_to_a_number()
        {
            var dr1 = new DateTimeRange(d4, d2);
            var dr2 = new DateTimeRange(d3, d2);

            (dr1 > dr2).ShouldBeTrue();
        }

        #endregion

    }

}
