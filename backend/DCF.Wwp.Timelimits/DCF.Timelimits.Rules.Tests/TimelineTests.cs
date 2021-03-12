using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using DCF.Common.Dates;
using DCF.Core;
using DCF.Timelimits.Rules.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DCF.Timelimits.Rules.Tests
{
    [TestClass]
    public class TimelineTests
    {
        private Mock<Timeline> timelineMock;

        [TestInitialize]
        public void init()
        {
            this.timelineMock = new Mock<Timeline>();
            this.timelineMock.CallBase = true;
        }

        [TestMethod]
        public void GetExtensionMonths_should_be_empty_without_extensions()
        {
            var extMonths = this.timelineMock.Object.GetExtensionMonths(ClockTypes.State);
            Assert.IsTrue(extMonths.Empty());
        }

        [TestMethod]
        [Description("")]
        public void GetExtensionMonths_should_count_state_extensions_months()
        {
            var extension1 = new Extension(ApplicationContext.Current.Date,ApplicationContext.Current.Date.AddMonths(5),ClockTypes.State,ExtensionDecision.Approve);

            this.timelineMock.Setup(t => t.GetExtensions(It.IsAny<ClockTypes>())).Returns(new List<Extension> {extension1});

            var stateMonths1 = Enumerable.Range(1, 60).Select(x => new TimelineMonth(ApplicationContext.Current.Date.AddMonths(-x)){ClockTypes = ClockTypes.State | ClockTypes.Federal | ClockTypes.CSJ});
            this.timelineMock.Setup(t => t.GetTimelineMonths(It.IsAny<ClockTypes?>())).Returns(stateMonths1);
            // 60 state months and 6 future extension months starting next month should yeild 0 extension months
            Assert.AreEqual(6, this.timelineMock.Object.GetExtensionMonths(ClockTypes.State).Count);

            // 1 month elapsed in extension
            var extension2 = new Extension(extension1.DateRange.Start.AddMonths(-1),extension1.DateRange.End.AddMonths(-1),extension1.ClockType,extension1.ExtensionDecision);
            this.timelineMock.Setup(t => t.GetExtensions(It.IsAny<ClockTypes>())).Returns(new List<Extension> { extension2 });
            Assert.AreEqual(5, this.timelineMock.Object.GetExtensionMonths(ClockTypes.State).Count);

            // All but 1 month elapsed in extension
            var extension3 = new Extension(ApplicationContext.Current.Date.AddMonths(-5),ApplicationContext.Current.Date, ClockTypes.State, ExtensionDecision.Approve);
            this.timelineMock.Setup(t => t.GetExtensions(It.IsAny<ClockTypes>())).Returns(new List<Extension> { extension3 });
            Assert.AreEqual(1, this.timelineMock.Object.GetExtensionMonths(ClockTypes.State).Count);

            // completely elapsed extension(s) shouldn't have any extension months remaining
            var extension4 = new Extension(ApplicationContext.Current.Date.AddMonths(-6),ApplicationContext.Current.Date.AddMonths(-1), ClockTypes.State, ExtensionDecision.Approve);
            var extension5 = new Extension(ApplicationContext.Current.Date.AddMonths(-13),ApplicationContext.Current.Date.AddMonths(-7), ClockTypes.State, ExtensionDecision.Approve);
            this.timelineMock.Setup(t => t.GetExtensions(It.IsAny<ClockTypes>())).Returns(new List<Extension> { extension4, extension5 });
            Assert.AreEqual(1, this.timelineMock.Object.GetExtensionMonths(ClockTypes.State).Count);
            Assert.AreEqual(1, this.timelineMock.Object.GetExtensionMonths(ClockTypes.State).Count);

            // 3 months left on current extension and 6 on subsequent extension
            var extension6 = new Extension(ApplicationContext.Current.Date.AddMonths(-2), ApplicationContext.Current.Date.AddMonths(2), ClockTypes.State, ExtensionDecision.Approve);
            var extension7 = new Extension(ApplicationContext.Current.Date.AddMonths(3), ApplicationContext.Current.Date.AddMonths(8), ClockTypes.State, ExtensionDecision.Approve);
            this.timelineMock.Setup(t => t.GetExtensions(It.IsAny<ClockTypes>())).Returns(new List<Extension> { extension6, extension7 });
            Assert.AreEqual(9, this.timelineMock.Object.GetExtensionMonths(ClockTypes.State).Count);

            //Ignore irrelevant extension
            extension6.ClockType = ClockTypes.CSJ;
            this.timelineMock.Setup(t => t.GetExtensionsByClockType(It.Is<ClockTypes>(x=>x == ClockTypes.State))).Returns(new List<Extension> { extension7 });
            this.timelineMock.Setup(t => t.GetExtensionsByClockType(It.Is<ClockTypes>(x=>x == ClockTypes.CSJ))).Returns(new List<Extension> { extension6 });
            this.timelineMock.Setup(t => t.GetExtensions(It.IsAny<ClockTypes>())).CallBase();
            Assert.AreEqual(6, this.timelineMock.Object.GetExtensionMonths(ClockTypes.State).Count);
        }

        [TestMethod]
        [Description("Timeline.GetExtensionMonths should count state extensions months correctly")]
        public void GetExtensionMonths_state_placement_extension_tests()
        {

        }
    }
}
