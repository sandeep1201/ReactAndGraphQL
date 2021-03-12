using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
	public interface IChildCareChildWithDisability : ICommonModel, ICloneable
	{
		Int32 ChildCareSectionId { get; set; }
		String Name { get; set; }
		Int32? Age { get; set; }
		String Details { get; set; }
	}
}
