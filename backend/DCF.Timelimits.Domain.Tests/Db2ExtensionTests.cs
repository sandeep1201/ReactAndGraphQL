using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCF.Timelimits.Rules.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DCF.Timelimits.Domain.Tests
{
    [TestClass]
    public class Db2ExtensionTests
    {
        [TestMethod]
        public void NullableTest()
        {
            Int32? a = null;

            Should.NotThrow(() =>
            {
                ClockTypes b = (ClockTypes) a.GetValueOrDefault();
            });

            Should.Throw<InvalidOperationException>(() =>
            {
                var bx = (ClockTypes) a.Value;
            }).Message.ShouldContain("Nullable object must have a value.");


            Should.Throw<InvalidOperationException>(() =>
            {
                var bx = a.Value;
            }).Message.ShouldContain("Nullable object must have a value.");

            Should.Throw<InvalidOperationException>(() =>
            {
                var bx = (ClockTypes)a;
            }).Message.ShouldContain("Nullable object must have a value");

            var wait = true;
        }
    }
}
