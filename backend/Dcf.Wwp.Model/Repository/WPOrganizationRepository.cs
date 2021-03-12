using Dcf.Wwp.Model.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IWPOrganizationRepository
    {
        public IEnumerable<IWPOrganization> GetWPOrganizations()
        {
            return _db.WPOrganizations.AsNoTracking().ToList<IWPOrganization>();
        }
    }
}