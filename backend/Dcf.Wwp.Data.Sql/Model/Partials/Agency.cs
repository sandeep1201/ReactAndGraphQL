using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Agency : IAgency
    {
        ICollection<IWorker> IAgency.Workers
        {
            get { return Workers.Cast<IWorker>().ToList(); }

            set { Workers = (ICollection<Worker>) value; }
        }

        ICollection<IWPOrganization> IAgency.WPOrganizations
        {
            get { return WPOrganizations.Cast<IWPOrganization>().ToList(); }

            set { WPOrganizations = (ICollection<WPOrganization>) value; }
        }

        ICollection<IContractor> IAgency.Contractors
        {
            get { return Contractors.Cast<IContractor>().ToList(); }

            set { Contractors = value as ICollection<Contractor>; }
        }
    }
}
