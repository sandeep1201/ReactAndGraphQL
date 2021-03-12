using System;

namespace Dcf.Wwp.Model.Interface
{
	public interface IWorkProgram : ICommonModel
	{
		String Name { get; set; }
        Int32? SortOrder { get; set; }
    }
}