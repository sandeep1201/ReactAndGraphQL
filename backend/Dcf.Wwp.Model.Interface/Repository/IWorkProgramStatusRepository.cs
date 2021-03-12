using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
	public interface IWorkProgramStatusRepository
	{
		IWorkProgramStatus WorkProgramStatusByOrder(Int32? sortOrder);

		IEnumerable<IWorkProgramStatus> WorkProgramStatuses();
	}
}
