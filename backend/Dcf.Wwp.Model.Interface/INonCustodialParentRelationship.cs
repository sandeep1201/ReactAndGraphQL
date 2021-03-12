using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface INonCustodialParentRelationship : ICommonModel, ICloneable
    {
        int SortOrder { get; set; }
        string Name { get; set; }
        bool IsDeleted { get; set; }
        ICollection<INonCustodialCaretaker> NonCustodialCaretakers { get; set; }
    }
}
