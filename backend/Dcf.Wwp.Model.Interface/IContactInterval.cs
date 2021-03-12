using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IContactInterval : ICommonDelModel
    {
        String Name { get; set; }
        Int32? SortOrder { get; set; }
        ICollection<INonCustodialCaretaker> NonCustodialCaretakers { get; set; }
        ICollection<INonCustodialChild> NonCustodialChilds { get; set; }
    }
}
