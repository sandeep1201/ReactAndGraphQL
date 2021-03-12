using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Model.Repository
{
	public partial class Repository
	{
		public IWorkProgramStatus WorkProgramStatusByOrder(int? sortOrder)
		{
			var workProgramStatus = (from wps in _db.WorkProgramStatuses where wps.SortOrder == sortOrder select wps).SingleOrDefault();
			return workProgramStatus;
		}

		public IEnumerable<IWorkProgramStatus> WorkProgramStatuses()
		{
			var q = from x in _db.WorkProgramStatuses orderby x.SortOrder select x;
			return q;
		}
	}
}
