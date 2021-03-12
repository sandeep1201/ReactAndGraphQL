using Dcf.Wwp.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public IWorkProgram WorkProgramById(int? id)
        {
            var workProgram = (from wp in _db.WorkPrograms where wp.Id == id select wp).SingleOrDefault();
            return workProgram;
        }

        public IWorkProgram OtherProgram(int? id)
        {
            var workProgram = (from wp in _db.WorkPrograms where wp.Id == id && wp.Name == "Other" select wp).SingleOrDefault();
            return workProgram;
        }
        public IEnumerable<IWorkProgram> WorkPrograms()
		{
			var q = from x in _db.WorkPrograms orderby x.SortOrder select x;
			return q;
		}
	}
}
