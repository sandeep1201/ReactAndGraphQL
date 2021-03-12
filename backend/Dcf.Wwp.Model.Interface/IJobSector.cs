using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IJobSector : ICommonModel,ICloneable
    {
        String Name { get; set; }
        Int32? SortOrder { get; set; }
        Boolean IsDeleted { get; set; }
        ICollection<IOtherJobInformation> OtherJobInformations { get; set; }
    }
}
