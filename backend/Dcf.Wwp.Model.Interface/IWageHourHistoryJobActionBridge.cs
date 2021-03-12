using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
	public interface IWageHourHistoryJobActionBridge : ICommonModel, ICloneable
	{
		Int32? WageHourHistoryId { get; set; }
		Int32? SortOrder { get; set; }
		Boolean IsDeleted { get; set; }
		Int32? WageActionId { get; set; }
		IWageHourHistory WageHourHistory { get; set; }
		IWageAction WageAction { get; set; }
	}
}
