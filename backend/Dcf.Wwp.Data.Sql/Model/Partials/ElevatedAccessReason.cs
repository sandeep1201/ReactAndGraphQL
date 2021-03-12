using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ElevatedAccessReason : BaseCommonModel, IElevatedAccessReason
    {
        ICollection<IElevatedAccess> IElevatedAccessReason.ElevatedAccesses
        {
            get { return ElevatedAccesses.Cast<IElevatedAccess>().ToList(); }
            set { ElevatedAccesses = value.Cast<ElevatedAccess>().ToList(); }
        }
    }
}
