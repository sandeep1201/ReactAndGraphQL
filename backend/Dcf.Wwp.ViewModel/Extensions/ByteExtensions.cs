using System;

namespace Dcf.Wwp.Api.Library.Extensions
{
	public static class ByteExtensions
	{
		public static string ToJsonValue(this byte[] value)
		{
			if (value == null)
				return null;

			var values = String.Join(",", value);

			return $"[{values}]";
        }
	}
}