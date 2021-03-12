using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IApplicationStatusType : ICommonDelModel, ICloneable
    {
        String ApplicationStatusName { get; set; }
        Boolean? IsRequired { get; set; }
        Int32 SortOrder { get; set; }
        ICollection<IFamilyBarriersSection> FamilyBarriersSections { get; set; }
    }
}
