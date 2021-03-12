using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.Common.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckIfDecimal()
        {

            decimal result;
            var testreslt = decimal.TryParse("", out result);
            var testresult_1 = decimal.TryParse(".", out result);
            var testresult_2 = decimal.TryParse("0.00", out result);
            var testresult_3 = decimal.TryParse("123", out result);
            // the value was decimal
            Assert.IsFalse(testreslt);
            Assert.IsFalse(testresult_1);
            Assert.IsTrue(testresult_2);
            Assert.IsTrue(testresult_3);
        }
    }
}
