﻿using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
	public interface IWorkProgramStatus : ICommonModel
	{
		Int32 SortOrder { get; set; }
		String Name { get; set; }

		ICollection<IInvolvedWorkProgram> InvolvedWorkPrograms { get; set; }
	}
}