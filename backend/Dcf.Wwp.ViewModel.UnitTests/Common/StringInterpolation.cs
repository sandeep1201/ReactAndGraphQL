using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.ViewModel.UnitTests.Common
{
    [TestClass]
    public class StringInterpolation
    {
    
        [TestMethod]
        public void SimpleInterpolation()
        {
            string name = "Mark";
            var    date = DateTime.Now;

            // Composite formatting:
            Debug.WriteLine("Hello, {0}! Today is {1}, it's {2:HH:mm} now.", name, date.DayOfWeek, date);
            // String interpolation:
            Debug.WriteLine($"Hello, {name}! Today is {date.DayOfWeek}, it's {date:HH:mm} now.");
            // Both calls produce the same output that is similar to:
            // Hello, Mark! Today is Wednesday, it's 19:40 now.
        }
    }
}
