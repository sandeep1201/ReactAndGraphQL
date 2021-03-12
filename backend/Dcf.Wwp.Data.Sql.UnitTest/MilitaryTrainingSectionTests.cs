using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Xunit;

namespace Dcf.Wwp.Data.Sql.UnitTest
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class MilitaryTrainingSectionTests
	{
		[Fact]
		public void Equals_WithOneNullId_ExpectFalse()
		{
			var mts1 = CreateStandardMilitaryTrainingSection();
			var mts2 = CreateStandardMilitaryTrainingSection();

			mts1.Id = 1;
			mts2.Id = 0;

			Assert.False(mts1.Equals(mts2));
		}

	    private MilitaryTrainingSection CreateStandardMilitaryTrainingSection()
	    {
		    var mts = new MilitaryTrainingSection();

		    mts.Id = 1;

		    return mts;
	    }

	}
}
