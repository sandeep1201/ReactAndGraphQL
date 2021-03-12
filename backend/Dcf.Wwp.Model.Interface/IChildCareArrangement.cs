using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IChildCareArrangement : ICommonDelModel, ICloneable
    {
        String Name { get; set; }
        Int32 SortOrder { get; set; }

        // No need for this nav property
        //ICollection<IChildCareChild> ChildCareChilds { get; set; }
    }
}
