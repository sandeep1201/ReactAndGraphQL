using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
	public interface IBenefitsOfferedAction : ICloneable, ICommonModel
	{
		String Name { get; set; }
		String ActionType { get; set; }
		Boolean? IsRequired { get; set; }
		Int32? SortOrder { get; set; }
		Boolean IsDeleted { get; set; }
		//ICollection<IJobBenefitsOfferedActionBridge> JobBenefitsOfferedActionBridges { get; set; }
	}
}
