using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.UnitTest.Api.Library.Domains
{
    [TestClass]
    public class WeeklyHoursWorkedDomainTest
    {
        [TestMethod]
        public void CalculateTotalSubsidyAmount_WeeklyHoursWorkedStartDateIsGreaterThanWageHoursEffectiveDate_UsesMostRecentSubsidyRate()
        {
            var wageHours = new List<(DateTime? EffectiveDate, decimal? HourlySubsidyRate, decimal? WorkSiteContribution)>
                            {
                                { (DateTime.Parse("10/10/2020"), 10.5M, 20.0M) },
                                { (DateTime.Parse("10/09/2020"), 7.0M, 10.0M) },
                                { (DateTime.Parse("10/08/2020"), 14.0M, 30.0M) }
                            };
            var weeklyHoursWorked = new WeeklyHoursWorked
                                    {
                                        Hours     = 22.0M,
                                        StartDate = DateTime.Parse("10/11/2020")
            };

            PrivateObject privateObject      = new PrivateObject(new WeeklyHoursWorkedDomain(null, null, null, null, null));
            var           totalSubsidyAmount = privateObject.Invoke("CalculateTotalSubsidyAmount", wageHours, weeklyHoursWorked);
            Assert.AreEqual(231.0M, totalSubsidyAmount);

        }
        [TestMethod]
        public void CalculateTotalSubsidyAmount_WeeklyHoursWorkedStartDateHasWageHoursEffectiveDateInCurrentWeek_UsesMostRecentSubsidyRate()
        {
            var wageHours = new List<(DateTime? EffectiveDate, decimal? HourlySubsidyRate, decimal? WorkSiteContribution)>
                            {
                                { (DateTime.Parse("10/15/2020"), 10.5M, 20.0M) },
                                { (DateTime.Parse("10/16/2020"), 7.0M, 10.0M) },
                                { (DateTime.Parse("10/17/2020"), 14.0M, 30.0M) }
                            };
            var weeklyHoursWorked = new WeeklyHoursWorked
                                    {
                                        Hours     = 22.0M,
                                        StartDate = DateTime.Parse("10/11/2020")
                                    };

            PrivateObject privateObject      = new PrivateObject(new WeeklyHoursWorkedDomain(null, null, null, null, null));
            var           totalSubsidyAmount = privateObject.Invoke("CalculateTotalSubsidyAmount", wageHours, weeklyHoursWorked);
            Assert.AreEqual(308.0M, totalSubsidyAmount);

        }
        [TestMethod]
        public void CalculateTotalSubsidyAmount_WeeklyHoursWorkedStartDateHasWageHoursEffectiveDateInCurrentWeekOrPastWeeks_UsesMostRecentSubsidyRate()
        {
            var wageHours = new List<(DateTime? EffectiveDate, decimal? HourlySubsidyRate, decimal? WorkSiteContribution)>
                            {
                                { (DateTime.Parse("10/18/2020"), 10.5M, 20.0M) },
                                { (DateTime.Parse("10/19/2020"), 7.0M, 10.0M) },
                                { (DateTime.Parse("10/20/2020"), 14.0M, 30.0M) }
                            };
            var weeklyHoursWorked = new WeeklyHoursWorked
                                    {
                                        Hours     = 22.0M,
                                        StartDate = DateTime.Parse("10/11/2020")
                                    };

            PrivateObject privateObject      = new PrivateObject(new WeeklyHoursWorkedDomain(null, null, null, null, null));
            var           totalSubsidyAmount = privateObject.Invoke("CalculateTotalSubsidyAmount", wageHours, weeklyHoursWorked);
            Assert.AreEqual(0.0M, totalSubsidyAmount);

        }

        [TestMethod]
        public void CalculateWorkSiteContributionAmount_WeeklyHoursWorkedStartDateIsGreaterThanWageHoursEffectiveDate_UsesMostRecentSubsidyRate()
        {
            var wageHours = new List<(DateTime? EffectiveDate, decimal? HourlySubsidyRate, decimal? WorkSiteContribution)>
                            {
                                { (DateTime.Parse("10/10/2020"), 10.5M, 20.0M) },
                                { (DateTime.Parse("10/09/2020"), 7.0M, 10.0M) },
                                { (DateTime.Parse("10/08/2020"), 14.0M, 30.0M) }
                            };
            var weeklyHoursWorked = new WeeklyHoursWorked
                                    {
                                        Hours     = 22.0M,
                                        StartDate = DateTime.Parse("10/11/2020")
                                    };

            PrivateObject privateObject      = new PrivateObject(new WeeklyHoursWorkedDomain(null, null, null, null, null));
            var totalWorkSiteAmount = privateObject.Invoke("CalculateWorkSiteContributionAmount", wageHours, weeklyHoursWorked);
            Assert.AreEqual(440.0M, totalWorkSiteAmount);

        }

        [TestMethod]
        public void CalculateWorkSiteContributionAmount_WeeklyHoursWorkedStartDateHasWageHoursEffectiveDateInCurrentWeek_UsesMostRecentSubsidyRate()
        {
            var wageHours = new List<(DateTime? EffectiveDate, decimal? HourlySubsidyRate, decimal? WorkSiteContribution)>
                            {
                                { (DateTime.Parse("10/15/2020"), 10.5M, 20.0M) },
                                { (DateTime.Parse("10/16/2020"), 7.0M, 10.0M) },
                                { (DateTime.Parse("10/17/2020"), 14.0M, 30.0M) }
                            };
            var weeklyHoursWorked = new WeeklyHoursWorked
                                    {
                                        Hours     = 22.0M,
                                        StartDate = DateTime.Parse("10/11/2020")
                                    };

            PrivateObject privateObject      = new PrivateObject(new WeeklyHoursWorkedDomain(null, null, null, null, null));
            var totalWorkSiteAmount = privateObject.Invoke("CalculateWorkSiteContributionAmount", wageHours, weeklyHoursWorked);
            Assert.AreEqual(660.0M, totalWorkSiteAmount);

        }

        [TestMethod]
        public void CalculateWorkSiteContributionAmount_WeeklyHoursWorkedStartDateHasWageHoursEffectiveDateInCurrentWeekOrPastWeeks_UsesMostRecentWorkSiteContribution()
        {
            var wageHours = new List<(DateTime? EffectiveDate, decimal? HourlySubsidyRate, decimal? WorkSiteContribution)>
                            {
                                { (DateTime.Parse("10/18/2020"), 10.5M, 20.0M) },
                                { (DateTime.Parse("10/19/2020"), 7.0M, 10.0M) },
                                { (DateTime.Parse("10/20/2020"), 14.0M, 30.0M) }
                            };
            var weeklyHoursWorked = new WeeklyHoursWorked
                                    {
                                        Hours     = 22.0M,
                                        StartDate = DateTime.Parse("10/11/2020")
                                    };

            PrivateObject privateObject      = new PrivateObject(new WeeklyHoursWorkedDomain(null, null, null, null, null));
            var           totalWorkSiteAmount = privateObject.Invoke("CalculateWorkSiteContributionAmount", wageHours, weeklyHoursWorked);
            Assert.AreEqual(0.0M, totalWorkSiteAmount);

        }
    }
}

