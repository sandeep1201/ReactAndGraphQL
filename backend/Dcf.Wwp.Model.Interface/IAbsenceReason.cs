using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IAbsenceReason : ICommonModel, ICloneable
    {
        String Name { get; set; }
        Int32? SortOrder { get; set; }
        Boolean IsDeleted { get; set; }
        ICollection<IAbsence> Absences { get; set; }
    }
}
