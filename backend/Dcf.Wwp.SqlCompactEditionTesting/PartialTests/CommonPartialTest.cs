using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Xunit;

namespace Dcf.Wwp.SqlCompactEditionTesting.PartialTests
{
	public class CommonPartialTest : BaseCommonModel
	{

		// Returns False when the two objects are not Equal and
		// returns True when both objects are Equal.
		private bool? TestMethod(string Name, string otherName)
		{

			if (!AdvEqual(Name, otherName))
			{
				return false;
			}

			return true;
		}
		[Fact]
		public void Equals_WhenOtherIsNull_ReturnsFalse()
		{

			string otherName = "Sohinder";
			string Name = null;


			bool? returnValue = TestMethod(Name, otherName);


			Assert.True(returnValue == false);
		}

		[Fact]
		public void Equals_WhenModelContextIsNull_ReturnsFalse()
		{

			string otherName = null;
			string Name = "Sohinder";


			bool? returnValue = TestMethod(Name, otherName);



			Assert.True(returnValue == false);
		}

		[Fact]
		public void Equals_WhenBothValuesAreNull_ReturnsTrue()
		{

			string otherName = null;
			string Name = null;

			bool? returnValue = TestMethod(Name, otherName);

			Assert.True(returnValue == true);
		}

		[Fact]
		public void Equals_WhenBothValuesAreEqual_ReturnsTrue()
		{

			string otherName = "Sohinder";
			string Name = "Sohinder";


			bool? returnValue = TestMethod(Name, otherName);


			Assert.True(returnValue == true);
		}

		[Fact]
		public void Equals_WhenModelContextStringDiffers_ReturnsFalse()
		{

			string otherName = "Sohinder";
			string Name = "Sohinder1";


			bool? returnValue = TestMethod(Name, otherName);


			Assert.True(returnValue == false);
		}


		[Fact]
		public void Equals_WhenOtherStringDiffers_ReturnsFalse()
		{

			string otherName = "Sohinder1";
			string Name = "Sohinder";


			bool? returnValue = TestMethod(Name, otherName);


			Assert.True(returnValue == false);
		}

	}
}
