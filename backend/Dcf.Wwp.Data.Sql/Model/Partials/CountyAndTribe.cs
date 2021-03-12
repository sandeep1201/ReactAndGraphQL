using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class CountyAndTribe : BaseEntity, ICountyAndTribe
    {
        ICollection<IOffice> ICountyAndTribe.Offices
        {
            get { return Offices.Cast<IOffice>().ToList(); }

            set { Offices = value.Cast<Office>().ToList(); }
        }
    }
}
