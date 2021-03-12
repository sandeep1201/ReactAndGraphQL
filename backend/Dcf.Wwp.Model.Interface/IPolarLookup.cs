using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
	public interface IPolarLookup : ICloneable
	{
		Int32 Id { get; set; }
		String Name { get; set; }
    }
}