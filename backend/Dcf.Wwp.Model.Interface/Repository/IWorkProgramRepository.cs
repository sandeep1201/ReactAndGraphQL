using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
	public interface IWorkProgramRepository
	{
		IWorkProgram WorkProgramById(Int32? id);

	    IWorkProgram OtherProgram(Int32? id);

        IEnumerable<IWorkProgram> WorkPrograms();
	}
}
