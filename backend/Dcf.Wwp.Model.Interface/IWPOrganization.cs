using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWPOrganization
    {
        int Id { get; set; }
        Nullable<int> AgencyId { get; set; }
        Nullable<int> CountyAndTribeId { get; set; }
        Nullable<int> officeId { get; set; }
        bool IsDeleted { get; set; }
        string ModifiedBy { get; set; }
        Nullable<System.DateTime> ModifiedDate { get; set; }
        byte[] RowVersion { get; set; }
        IAgency Agency { get; set; }
        ICountyAndTribe CountyAndTribe { get; set; }
    }
}
