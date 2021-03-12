using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IExamPassType : ICommonModel
    {
        Int32? SortOrder { get; set; }
        String Name { get; set; }
        ICollection<IExamResult> ExamResults { get; set; }

    }
}
