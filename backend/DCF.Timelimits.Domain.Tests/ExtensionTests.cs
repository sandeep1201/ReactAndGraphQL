using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Timelimits.Rules.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DCF.Timelimits.Domain.Tests
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void ShouldNotBeElapsedWhenSartingEndingInFuture()
        {
            var extension = new Extension(ClockTypes.State, DateTime.Now,new DateTime(2017,1,1),DateTime.Now.AddMonths(1));
            extension.HasElapsed.ShouldBeFalse();
        }
    }

    [TestClass]
    public class ExtensionSequenceTests
    {
        [TestMethod]
        public void ShouldReturnCurrentExtensionWhenOnly1FutureExtension()
        {
            var extension = new Extension(ClockTypes.State, DateTime.Now, new DateTime(2017, 1, 1), DateTime.Now.AddMonths(1));
            var extensionSequence = new ExtensionSequence(1,new List<Extension>() {extension});
            extensionSequence.CurrentExtension.ShouldNotBeNull();
            extensionSequence.CurrentExtension.ShouldBe(extension);
        }

        [TestMethod]
        public void ShouldReturnCurrentExtensionWhenOnlyFutureExtensionAndOneElapsed()
        {
            var extension1 = new Extension(ClockTypes.State, DateTime.Now, new DateTime(2017, 1, 1), DateTime.Now.AddMonths(-1));
            var extension2 = new Extension(ClockTypes.State, DateTime.Now, DateTime.Now.StartOf(DateTimeUnit.Month), DateTime.Now.AddMonths(6));
            var extensionSequence = new ExtensionSequence(1, new List<Extension>() { extension1, extension2 });
            extensionSequence.CurrentExtension.ShouldNotBeNull();
            extensionSequence.CurrentExtension.ShouldBe(extension2);
        }
    }
}
