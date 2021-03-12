using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SSNType : BaseEntity, ISSNType
    {
        ICollection<IAKA> ISSNType.AKAs
        {
            get { return AKAs.Cast<IAKA>().ToList(); }
            set { AKAs = value.Cast<AKA>().ToList(); }
        }
    }
}
