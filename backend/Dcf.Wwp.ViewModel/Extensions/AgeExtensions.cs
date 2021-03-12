using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Extensions
{
	public static class AgeExtensions
	{
		public static int ToAgebyMonthDayYear(this DateTime? input)
		{
			if (input == null)
				return 0;

			var today = DateTime.Today;
			int age = today.Year - input.Value.Year;

			if (input > today.AddYears(-age))
				age--;

			return age;
		}
	}
}
